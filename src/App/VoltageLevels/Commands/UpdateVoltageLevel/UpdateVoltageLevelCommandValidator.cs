using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.VoltageLevels.Commands.UpdateVoltageLevel;

public class UpdateVoltageLevelCommandValidator : AbstractValidator<UpdateVoltageLevelCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateVoltageLevelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Level)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateVoltageLevelCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.VoltageLevels
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Level != title, cancellationToken);
    }

}
