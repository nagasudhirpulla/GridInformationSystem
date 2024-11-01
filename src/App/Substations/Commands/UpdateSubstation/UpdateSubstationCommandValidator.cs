using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Commands.UpdateSubstation;

public class UpdateSubstationCommandValidator : AbstractValidator<UpdateSubstationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSubstationCommandValidator(IApplicationDbContext context)
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
    public async Task<bool> BeUniqueLocationVoltage(UpdateSubstationCommand cmd, CancellationToken cancellationToken)
    {
        var isVoltLocPresent = await _context.Substations
            .AnyAsync(s => (s.Id != cmd.Id) && (s.LocationId == cmd.LocationId) && (s.VoltageLevelId == cmd.VoltageLevelId), cancellationToken);
        return !isVoltLocPresent;
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