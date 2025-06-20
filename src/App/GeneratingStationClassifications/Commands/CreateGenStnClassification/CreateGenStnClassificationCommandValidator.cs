using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationClassifications.Commands.CreateGenStnClassification;

public class CreateGenStnClassificationCommandValidator : AbstractValidator<CreateGenStnClassificationCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateGenStnClassificationCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Classification)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.GeneratingStationClassifications
            .AllAsync(l => l.Classification != name, cancellationToken);
    }
}
