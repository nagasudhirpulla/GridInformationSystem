using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.BusReactors.Commands.UpdateBusReactor;

public class UpdateBusReactorCommandValidator : AbstractValidator<UpdateBusReactorCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBusReactorCommandValidator(IApplicationDbContext context)
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
            .MustAsync(BeInSameSubstation)
                .WithMessage("The bus reactor should be present in the same substation")
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

    public async Task<bool> BeUniqueBusReactorInSubstation(UpdateBusReactorCommand cmd, CancellationToken cancellationToken)
    {
        bool sameBrExists = await _context.BusReactors
            .AnyAsync(l => (l.Id != cmd.Id) && (l.BusId == cmd.BusId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameBrExists;
    }

    public async Task<bool> BeInSameSubstation(UpdateBusReactorCommand cmd, CancellationToken cancellationToken)
    {
        Bus newBus = await _context.Buses.FirstOrDefaultAsync(b => b.Id == cmd.BusId, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        BusReactor existingBr = await _context.BusReactors.FirstOrDefaultAsync(br => br.Id == cmd.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        bool isBrInSameSubstation = existingBr.Substation1Id == newBus.Substation1Id;
        return !isBrInSameSubstation;
    }
}
