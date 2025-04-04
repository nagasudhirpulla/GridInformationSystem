using App.Common.Interfaces;
using App.Owners.Utils;
using App.Substations.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcPoles.Commands.CreateHvdcPole;

public class CreateHvdcPoleCommandValidator : AbstractValidator<CreateHvdcPoleCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateHvdcPoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueHvdcPoleInSubstation)
                .WithMessage("The combination of HvdcPole number and Substation should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => !cmd.DeCommissioningDate.HasValue || (cmd.DeCommissioningDate > cmd.CommissioningDate))
                .WithMessage("Decommissioning date should be greater than Commissioning Date")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => cmd.CommercialOperationDate > cmd.CommissioningDate)
                .WithMessage("Commercial Operation Date date should be greater than Commissioning Date")
                .WithErrorCode("Unique");

        // check if substation is DC
        RuleFor(v => v.SubstationId)
            .MustAsync(BeNonAcSubstation)
                .WithMessage("The Substation should be DC substation");
    }

    public async Task<bool> BeUniqueHvdcPoleInSubstation(CreateHvdcPoleCommand cmd, CancellationToken cancellationToken)
    {
        bool sameHvdcPoleExists = await _context.HvdcPoles
            .AnyAsync(l => (l.Substation1Id == cmd.SubstationId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameHvdcPoleExists;
    }

    public async Task<bool> BeNonAcSubstation(int substationId, CancellationToken cancellationToken)
    {
        return !await SubstationUtils.IsAcSubstation(substationId, _context, cancellationToken);
    }
}