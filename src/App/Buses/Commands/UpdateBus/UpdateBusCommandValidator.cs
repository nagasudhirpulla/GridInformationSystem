using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Buses.Commands.UpdateBus;

public class UpdateBusCommandValidator : AbstractValidator<UpdateBusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBusCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueBusInSubstation)
                .WithMessage("The combination of Bus number, Bus type and Substation should be unique")
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

    public async Task<bool> BeUniqueBusInSubstation(UpdateBusCommand cmd, CancellationToken cancellationToken)
    {
        bool sameBusExists = await _context.Buses
            .AnyAsync(l => (l.Id!=cmd.Id) && (l.SubstationId1 == cmd.SubstationId) && (l.BusType == cmd.BusType) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameBusExists;
    }
}

