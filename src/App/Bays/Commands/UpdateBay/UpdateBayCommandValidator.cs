using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using Core.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Bays.Commands.UpdateBay;

public class UpdateBayCommandValidator : AbstractValidator<UpdateBayCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBayCommandValidator(IApplicationDbContext context)
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
                .WithMessage("The combination of Bay number, Bay type and Substation should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BayConnectedElementsInSameSubstation)
                .WithMessage("Both bay connected elements should be in same substation");

        RuleFor(v => v)
            .CustomAsync(CheckConnectedElementSemantics);

        RuleFor(v => v)
            .Must(cmd => !cmd.DeCommissioningDate.HasValue || (cmd.DeCommissioningDate > cmd.CommissioningDate))
                .WithMessage("Decommissioning date should be greater than Commissioning Date")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => cmd.CommercialOperationDate > cmd.CommissioningDate)
                .WithMessage("Commercial Operation Date date should be greater than Commissioning Date")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueBayInSubstation(UpdateBayCommand cmd, CancellationToken cancellationToken)
    {
        bool sameBayExists = await _context.Bays
            .AnyAsync(l => (l.Id != cmd.Id) && (l.BayType == cmd.BayType) && new HashSet<int> { cmd.Element1Id, cmd.Element2Id }.IsSupersetOf(new[] { l.Element1Id, l.Element2Id }), cancellationToken);
        return !sameBayExists;
    }

    public async Task<bool> BayConnectedElementsInSameSubstation(UpdateBayCommand cmd, CancellationToken cancellationToken)
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

    public async Task CheckConnectedElementSemantics(UpdateBayCommand cmd, ValidationContext<UpdateBayCommand> ctx, CancellationToken cancellationToken)
    {
        Element? el1 = await _context.Elements.FirstOrDefaultAsync(e => e.Id == cmd.Element1Id, cancellationToken: cancellationToken);
        Element? el2 = await _context.Elements.FirstOrDefaultAsync(e => e.Id == cmd.Element2Id, cancellationToken: cancellationToken);
        if ((el1 == null) || (el2 == null))
        {
            ctx.AddFailure("Both elements should be non null for Bay");
        }
        if (cmd.BayType == BayTypeEnum.TieBay)
        {
            if ((el1 is Bus) || (el2 is Bus))
            {
                ctx.AddFailure("For a Tie Bay Element1 and Element2 should be Non-Bus");
            }
        }
        else if (cmd.BayType == BayTypeEnum.MainBay)
        {
            if (((el1 is not Bus) && (el2 is not Bus)) || ((el1 is Bus) && (el2 is Bus)))
            {
                ctx.AddFailure("For a Main Bay exactly one of the element1 or element2 should be bus");
            }
        }
        else if (new List<BayTypeEnum> { BayTypeEnum.BusCouplerBay, BayTypeEnum.BusSectionalizerBay, BayTypeEnum.TBCBay }.Contains(cmd.BayType))
        {
            if ((el1 is not Bus) && (el2 is not Bus))
            {
                ctx.AddFailure("For a Bus Coupler, Bus Sectionalizer, TBC Bay elment1 and element2 both should be bus type");
            }
        }
    }
}