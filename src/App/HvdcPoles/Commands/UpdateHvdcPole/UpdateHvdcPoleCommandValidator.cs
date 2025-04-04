using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using App.Substations.Utils;

namespace App.HvdcPoles.Commands.UpdateHvdcPole;

public class UpdateHvdcPoleCommandValidator : AbstractValidator<UpdateHvdcPoleCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateHvdcPoleCommandValidator(IApplicationDbContext context)
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

        RuleFor(v => v.SubstationId)
            .MustAsync(BeNonAcSubstation)
                .WithMessage("The Substation should be DC substation")
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

    public async Task<bool> BeUniqueHvdcPoleInSubstation(UpdateHvdcPoleCommand cmd, CancellationToken cancellationToken)
    {
        bool sameHvdcPoleExists = await _context.HvdcPoles
            .AnyAsync(l => (l.Id != cmd.Id) && (l.Substation1Id == cmd.SubstationId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameHvdcPoleExists;
    }

    public async Task<bool> BeNonAcSubstation(int substationId, CancellationToken cancellationToken)
    {
        return !await SubstationUtils.IsAcSubstation(substationId, _context, cancellationToken);
    }
}
