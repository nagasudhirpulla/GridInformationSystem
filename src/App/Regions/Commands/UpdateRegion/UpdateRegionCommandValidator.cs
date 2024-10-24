using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Regions.Commands.UpdateRegion;

public class UpdateRegionCommandValidator : AbstractValidator<UpdateRegionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateRegionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateRegionCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Regions
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}
