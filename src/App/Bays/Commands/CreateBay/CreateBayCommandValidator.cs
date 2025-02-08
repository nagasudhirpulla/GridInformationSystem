using App.Buses.Commands.CreateBus;
using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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
            .Must(cmd => !cmd.DeCommissioningDate.HasValue || (cmd.DeCommissioningDate > cmd.CommissioningDate))
                .WithMessage("Decommissioning date should be greater than Commissioning Date")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => cmd.CommercialOperationDate > cmd.CommissioningDate)
                .WithMessage("Commercial Operation Date date should be greater than Commissioning Date")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueBayInSubstation(CreateBayCommand cmd, CancellationToken cancellationToken)
    {
        bool sameBusExists = await _context.Bays
            .AnyAsync(l => (l.Element1Id == cmd.Element1Id) && (l.Element2Id == cmd.Element2Id), cancellationToken);
        return !sameBusExists;
    }
}
