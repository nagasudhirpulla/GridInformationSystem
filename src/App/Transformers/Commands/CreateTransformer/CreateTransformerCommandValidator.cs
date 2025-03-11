using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Transformers.Commands.CreateTransformer;

public class CreateTransformerCommandValidator : AbstractValidator<CreateTransformerCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTransformerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.Substation1Id)
            .NotEmpty();

        RuleFor(v => v.Substation1Id)
            .NotEqual(l => l.Substation2Id);

        RuleFor(v => v.Substation2Id)
            .NotEmpty();

        RuleFor(v => v.MvaCapacity)
            .GreaterThanOrEqualTo(0);

        RuleFor(v => v.Substation1Id)
            .MustAsync(BeAnAcSubstation)
                .WithMessage("Substation 1 is not an AC Bus")
                .WithErrorCode("Invalid");

        RuleFor(v => v.Substation2Id)
            .MustAsync(BeAnAcSubstation)
                .WithMessage("Substation 2 is not an AC Bus")
                .WithErrorCode("Invalid");

        RuleFor(v => v)
            .MustAsync(BeDifferentVoltageSubstationsAtSameLocation)
                .WithMessage("Voltage levels of both substations to be different in same location")
                .WithErrorCode("Invalid");

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueTranformerBetweenSubstations)
                .WithMessage("The combination of From and To substations and transformer number should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .Must(cmd => !cmd.DeCommissioningDate.HasValue || cmd.DeCommissioningDate > cmd.CommissioningDate)
                .WithMessage("Decommissioning date should be greater than Commissioning Date")
                .WithErrorCode("Invalid");

        RuleFor(v => v)
            .Must(cmd => cmd.CommercialOperationDate > cmd.CommissioningDate)
                .WithMessage("Commercial Operation Date date should be greater than Commissioning Date")
                .WithErrorCode("Invalid");
    }

    public async Task<bool> BeUniqueTranformerBetweenSubstations(CreateTransformerCommand cmd, CancellationToken cancellationToken)
    {
        // combination of FromBus, ToBus and Element number is unique
        bool sameElementExists = await _context.Transformers
            .AnyAsync(l => new HashSet<int> { cmd.Substation1Id, cmd.Substation2Id }.IsSupersetOf(new[] { l.Substation1Id, l.Substation2Id ?? -1 }) && l.ElementNumber == cmd.ElementNumber, cancellationToken);
        return !sameElementExists;
    }

    public async Task<bool> BeAnAcSubstation(int substationId, CancellationToken cancellationToken)
    {
        return await Substations.Utils.SubstationUtils.IsAcSubstation(substationId, _context, cancellationToken);
    }

    public async Task<bool> BeDifferentVoltageSubstationsAtSameLocation(CreateTransformerCommand cmd, CancellationToken cancellationToken)
    {
        // sub1 and sub2 should have different voltage levels in same location
        Substation sub1 = await _context.Substations.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == cmd.Substation1Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        Substation sub2 = await _context.Substations.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == cmd.Substation2Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        bool isVoltLvlSame = sub1.VoltageLevel == sub2.VoltageLevel;
        bool isSameLocation = sub1.LocationId == sub2.LocationId;
        return isVoltLvlSame && isSameLocation;
    }
}
