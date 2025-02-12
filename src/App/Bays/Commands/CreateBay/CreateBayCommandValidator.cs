using App.Buses.Commands.CreateBus;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using Core.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace App.Bays.Commands.CreateBay;

public class CreateBayCommandValidator : AbstractValidator<CreateBayCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateBayCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueBayInSubstation)
                .WithMessage("The combination of Element 1 and Element 2 should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BayConnectedElementsInSameSubstation)
                .WithMessage("Both bay connected elements should be in same substation");

        RuleFor(v => v)
            .MustAsync(CheckConnectedElementSemantics)
                .WithMessage("Connected bay elements should follow semantics as per bay type");

        RuleFor(v => v)
            .Must(cmd => !cmd.DeCommissioningDate.HasValue || (cmd.DeCommissioningDate > cmd.CommissioningDate))
                .WithMessage("Decommissioning date should be greater than Commissioning Date");

        RuleFor(v => v)
            .Must(cmd => cmd.CommercialOperationDate > cmd.CommissioningDate)
                .WithMessage("Commercial Operation Date date should be greater than Commissioning Date");
    }

    public async Task<bool> BeUniqueBayInSubstation(CreateBayCommand cmd, CancellationToken cancellationToken)
    {
        bool sameBayExists = await _context.Bays
            .AnyAsync(l => (l.Element1Id == cmd.Element1Id) && (l.Element2Id == cmd.Element2Id), cancellationToken);
        return !sameBayExists;
    }

    public async Task<bool> BayConnectedElementsInSameSubstation(CreateBayCommand cmd, CancellationToken cancellationToken)
    {
        Element? el1 = await _context.Elements.FirstOrDefaultAsync(e => e.Id == cmd.Element1Id, cancellationToken: cancellationToken);
        Element? el2 = await _context.Elements.FirstOrDefaultAsync(e => e.Id == cmd.Element2Id, cancellationToken: cancellationToken);
        if ((el1 == null) || (el2 == null))
        {
            return false;
        }
        List<int> el1SubstnIds = [el1.Substation1Id];
        List<int> el2SubstnIds = [el2.Substation1Id];
        if (el1.Substation2Id != null)
        {
            el1SubstnIds = (List<int>)el1SubstnIds.Append(el1.Substation2Id.Value);
        }
        if (el2.Substation2Id != null)
        {
            el2SubstnIds = (List<int>)el2SubstnIds.Append(el2.Substation2Id.Value);
        }
        bool commonSubstnExists = el1SubstnIds.Any(el2SubstnIds.Contains);

        return commonSubstnExists;
    }

    public async Task<bool> CheckConnectedElementSemantics(CreateBayCommand cmd, CancellationToken cancellationToken)
    {
        // TODO get error message from this function
        Element? el1 = await _context.Elements.FirstOrDefaultAsync(e => e.Id == cmd.Element1Id, cancellationToken: cancellationToken);
        Element? el2 = await _context.Elements.FirstOrDefaultAsync(e => e.Id == cmd.Element2Id, cancellationToken: cancellationToken);
        if ((el1 == null) || (el2 == null))
        {
            return false;
        }
        if (cmd.BayType == BayTypeEnum.TieBay)
        {
            if ((el1 is Bus) || (el2 is Bus))
            {
                // "For a Tie Bay Element1 and Element2 should be Non-Bus"
                return false;
            }
        }
        else if (cmd.BayType == BayTypeEnum.MainBay)
        {
            if (((el1 is not Bus) && (el2 is not Bus)) || ((el1 is Bus) && (el2 is Bus)))
            {
                // "For a Main Bay exactly one of the element1 or element2 should be bus"
                return false;
            }
        }
        else if (new List<BayTypeEnum> { BayTypeEnum.BusCouplerBay, BayTypeEnum.BusSectionalizerBay, BayTypeEnum.TBCBay }.Contains(cmd.BayType))
        {
            if ((el1 is not Bus) && (el2 is not Bus))
            {
                // "For a Bus Coupler, Bus Sectionalizer, TBC Bay elment1 and element2 both are bus type"
                return false;
            }
        }
        return true;
    }
}
