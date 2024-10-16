using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.VoltageLevels.Commands.CreateVoltageLevel;

public class CreateVoltageLevelCommandValidator : AbstractValidator<CreateVoltageLevelCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateVoltageLevelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Level)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.VoltageLevels
            .AllAsync(l => l.Level != name, cancellationToken);
    }
}

