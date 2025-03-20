using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.LineReactors.Commands.UpdateLineReactor;

public class UpdateLineReactorCommandValidator : AbstractValidator<UpdateLineReactorCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLineReactorCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.LineId)
            .NotEmpty();

        RuleFor(v => v.MvarCapacity)
            .GreaterThan(0);

        RuleFor(v => v)
            .MustAsync(BeInLineSubstation)
                .WithMessage("Voltage levels of both buses is not same")
                .WithErrorCode("Invalid");

        RuleFor(v => v)
            .MustAsync(BeUniqueLineReactorInSubstation)
                .WithMessage("Voltage levels of both buses is not same")
                .WithErrorCode("Invalid");

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
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

    //todo correct this
    public async Task<bool> BeInLineSubstation(UpdateLineReactorCommand cmd, CancellationToken cancellationToken)
    {
        Line line = await _context.Lines.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == cmd.LineId, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();

        bool isLineReactorInLineSubstation = new List<int> { line.Substation1Id, line.Substation2Id ?? -1 }.Contains(cmd.SubstationId);
        return isLineReactorInLineSubstation;
    }

    public async Task<bool> BeUniqueLineReactorInSubstation(UpdateLineReactorCommand cmd, CancellationToken cancellationToken)
    {
        bool sameLrExists = await _context.LineReactors
            .AnyAsync(l => (l.Id != cmd.Id) && l.LineId.Equals(cmd.LineId) && l.Substation1Id.Equals(cmd.SubstationId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return sameLrExists;
    }
}
