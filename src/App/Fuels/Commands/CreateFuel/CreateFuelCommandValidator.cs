using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Fuels.Commands.CreateFuel;

public class CreateFuelCommandValidator : AbstractValidator<CreateFuelCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateFuelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Fuels
            .AllAsync(l => l.FuelName != name, cancellationToken);
    }
}