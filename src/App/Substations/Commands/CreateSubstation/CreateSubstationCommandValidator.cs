using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Commands.CreateSubstation;

public class CreateSubstationCommandValidator : AbstractValidator<CreateSubstationCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateSubstationCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.OwnerIds)
            .NotEmpty();

        RuleFor(v => v)
            .MustAsync(BeUniqueLocationVoltage)
                .WithMessage("The combination of voltage level and location should be unique")
                .WithErrorCode("Unique");

        RuleFor(v => v.OwnerIds)
            .Must(BeValidOwnerIds)
                .WithMessage("invalid owner Ids provided")
                .WithErrorCode("Unique");

    }
    public async Task<bool> BeUniqueLocationVoltage(CreateSubstationCommand cmd, CancellationToken cancellationToken)
    {
        return !await _context.Substations
            .AnyAsync(l => (l.LocationId == cmd.LocationId) && (l.VoltageLevelId == cmd.VoltageLevelId), cancellationToken);
    }

    public static bool BeValidOwnerIds(string oIds)
    {
        return oIds.Split(',').Any(oId =>
        {
            int ownerId = -1;
            try
            {
                ownerId = Int32.Parse(oId);
            }
            catch (FormatException)
            {

                return false;
            }
            return true;
        });
    }
}
