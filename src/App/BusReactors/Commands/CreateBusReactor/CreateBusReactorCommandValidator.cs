using App.Common.Interfaces;
using App.Owners.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.BusReactors.Commands.CreateBusReactor;

public class CreateBusReactorCommandValidator : AbstractValidator<CreateBusReactorCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateBusReactorCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueBusReactorInSubstation)
                .WithMessage("The combination of Bus reactor number and Bus should be unique")
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

    public async Task<bool> BeUniqueBusReactorInSubstation(CreateBusReactorCommand cmd, CancellationToken cancellationToken)
    {
        bool sameBusExists = await _context.BusReactors
            .AnyAsync(l => (l.BusId == cmd.BusId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameBusExists;
    }

}

