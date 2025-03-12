using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingUnits.Commands.UpdateGeneratingUnit;

public class UpdateGeneratingUnitCommandValidator : AbstractValidator<UpdateGeneratingUnitCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateGeneratingUnitCommandValidator(IApplicationDbContext context)
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
    }

    public async Task<bool> BeUniqueGenUnitInSubstation(UpdateGeneratingUnitCommand cmd, CancellationToken cancellationToken)
    {
        bool sameBusExists = await _context.GeneratingUnits
            .AnyAsync(l => (l.Id != cmd.Id) && (l.Substation1Id == cmd.GeneratingStationId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameBusExists;
    }
}
