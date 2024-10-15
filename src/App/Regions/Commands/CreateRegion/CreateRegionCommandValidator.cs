using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Regions.Commands.CreateRegion;

public class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateRegionCommandValidator(IApplicationDbContext context)
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
        return await _context.Regions
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
