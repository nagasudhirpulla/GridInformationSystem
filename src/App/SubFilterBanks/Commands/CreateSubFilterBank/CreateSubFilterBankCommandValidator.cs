using App.Common.Interfaces;
using App.Owners.Utils;
using Microsoft.EntityFrameworkCore;
using App.Substations.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace App.SubFilterBanks.Commands.CreateSubFilterBank;

public class CreateSubFilterBankCommandValidator : AbstractValidator<CreateSubFilterBankCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateSubFilterBankCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v.OwnerIds)
            .Must(OwnerUtils.BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

        RuleFor(v => v)
            .MustAsync(BeUniqueSubFilterBankInSubstation)
                .WithMessage("The combination of SubFilterBank number and Substation should be unique")
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
        RuleFor(v => v.FilterBankId)
            .MustAsync(BeNonAcSubstation)
                .WithMessage("The Substation should be DC substation");
    }

    public async Task<bool> BeUniqueSubFilterBankInSubstation(CreateSubFilterBankCommand cmd, CancellationToken cancellationToken)
    {
        bool sameSubFilterBankExists = await _context.SubFilterBanks
            .AnyAsync(l => (l.FilterBankId == cmd.FilterBankId) && (l.ElementNumber == cmd.SubFilterTag), cancellationToken);
        return !sameSubFilterBankExists;
    }

    public async Task<bool> BeNonAcSubstation(int filterBankId, CancellationToken cancellationToken)
    {
        // query filter bank from db
        var filterBank = await _context.FilterBanks
            .FirstOrDefaultAsync(s => s.Id == filterBankId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "FilterBank Id is not present in database"
                                                                                }]);
        return !await SubstationUtils.IsAcSubstation(filterBank.Substation1Id, _context, cancellationToken);
    }
}