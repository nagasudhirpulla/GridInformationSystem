using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingUnits.Commands.CreateGeneratingUnit;

public class CreateGeneratingUnitCommandValidator : AbstractValidator<CreateGeneratingUnitCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateGeneratingUnitCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueGenUnitInSubstation)
                .WithMessage("The combination of Unit number and GeneratingStation should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => !cmd.DeCommissioningDate.HasValue || (cmd.DeCommissioningDate > cmd.CommissioningDate))
                .WithMessage("Decommissioning date should be greater than Commissioning Date")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => cmd.CommercialOperationDate > cmd.CommissioningDate)
                .WithMessage("Commercial Operation Date date should be greater than Commissioning Date")
                .WithErrorCode("Unique");
        // TODO check if generating unit voltage is less that generating station voltage
    }

    public async Task<bool> BeUniqueGenUnitInSubstation(CreateGeneratingUnitCommand cmd, CancellationToken cancellationToken)
    {
        bool sameElExists = await _context.GeneratingUnits
            .AnyAsync(l => (l.Substation1Id == cmd.GeneratingStationId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameElExists;
    }
}
