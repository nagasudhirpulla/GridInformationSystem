using App.Common.Interfaces;
using App.Owners.Utils;
using App.Substations.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.FilterBanks.Commands.CreateFilterBank;

public class CreateFilterBankCommandValidator : AbstractValidator<CreateFilterBankCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateFilterBankCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueFilterBankInSubstation)
                .WithMessage("The combination of FilterBank number and Substation should be unique")
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
            .MustAsync(BeAcSubstation)
                .WithMessage("The Substation should be AC substation");

        RuleFor(v => v.Mvar)
            .GreaterThanOrEqualTo(0);
    }

    public async Task<bool> BeUniqueFilterBankInSubstation(CreateFilterBankCommand cmd, CancellationToken cancellationToken)
    {
        bool sameFilterBankExists = await _context.FilterBanks
            .AnyAsync(l => (l.Substation1Id == cmd.SubstationId) && (l.ElementNumber == cmd.ElementNumber), cancellationToken);
        return !sameFilterBankExists;
    }

    public async Task<bool> BeAcSubstation(int substationId, CancellationToken cancellationToken)
    {
        return await SubstationUtils.IsAcSubstation(substationId, _context, cancellationToken);
    }
}