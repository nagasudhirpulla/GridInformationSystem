using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Fuels.Commands.UpdateFuel;

public class UpdateFuelCommandValidator : AbstractValidator<UpdateFuelCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateFuelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.FuelName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateFuelCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Fuels
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}