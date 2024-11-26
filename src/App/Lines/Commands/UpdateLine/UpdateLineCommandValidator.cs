using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Lines.Commands.UpdateLine;

public class UpdateLineCommandValidator : AbstractValidator<UpdateLineCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLineCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.Bus1Id)
            .NotEmpty();

        RuleFor(v => v.Bus1Id)
            .NotEqual(l => l.Bus2Id);

        RuleFor(v => v.Bus2Id)
            .NotEmpty();

        RuleFor(v => v.Length)
            .GreaterThan(0);

        RuleFor(v => v.ConductorType)
            .NotEmpty();

        RuleFor(v => v.Bus1Id)
            .MustAsync(BeAnAcBus)
                .WithMessage("Bus is not an AC Bus")
                .WithErrorCode("Invalid");

        RuleFor(v => v)
            .MustAsync(BeSameVoltageLevelBuses)
                .WithMessage("Voltage levels of both buses is not same")
                .WithErrorCode("Invalid");

        RuleFor(v => v.Bus2Id)
            .MustAsync(BeAnAcBus)
                .WithMessage("Bus is not an AC Bus")
                .WithErrorCode("Invalid");

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueLinesBetweenSubstations)
                .WithMessage("The combination of From and To buses and line number should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => !cmd.DeCommissioningDate.HasValue || (cmd.DeCommissioningDate > cmd.CommissioningDate))
                .WithMessage("Decommissioning date should be greater than Commissioning Date")
                .WithErrorCode("Invalid");

        RuleFor(v => v)
            .Must(cmd => cmd.CommercialOperationDate > cmd.CommissioningDate)
                .WithMessage("Commercial Operation Date date should be greater than Commissioning Date")
                .WithErrorCode("Invalid");
    }

    public async Task<bool> BeUniqueLinesBetweenSubstations(UpdateLineCommand cmd, CancellationToken cancellationToken)
    {
        // combination of FromBus, ToBus and Element number is unique
        // todo use any to simplify bus1 bus2 comparision
        bool sameBusExists = await _context.Lines
            .AnyAsync(l => (l.Id != cmd.Id) && ((l.Bus1Id.Equals(cmd.Bus1Id) && l.Bus2Id.Equals(cmd.Bus2Id)) || (l.Bus2Id.Equals(cmd.Bus1Id) && l.Bus1Id.Equals(cmd.Bus2Id))) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameBusExists;
    }

    public async Task<bool> BeAnAcBus(int busId, CancellationToken cancellationToken)
    {
        return await Buses.Utils.IsAnAcBus.ExecuteAsync(busId, _context, cancellationToken);
    }

    public async Task<bool> BeSameVoltageLevelBuses(UpdateLineCommand cmd, CancellationToken cancellationToken)
    {
        // bus1 and bus2 should have same voltage levels
        Bus bus1 = await _context.Buses.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == cmd.Bus1Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        Bus bus2 = await _context.Buses.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == cmd.Bus2Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        //bus1.VoltageLevelCache
        bool isVoltLvlSame = bus1.VoltageLevelCache == bus2.VoltageLevelCache;
        return isVoltLvlSame;
    }
}