using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Locations.Commands.UpdateLocation;

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLocationCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(v => v.StateId)
            .MustAsync(async (int sId, CancellationToken cancellationToken) =>
            {
                return await _context.States.AnyAsync(s => s.Id == sId, cancellationToken);
            })
                .WithMessage("'{PropertyName}' does not exist.")
                .WithErrorCode("NotExists"); ;
    }

    public async Task<bool> BeUniqueName(UpdateLocationCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Locations
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}
