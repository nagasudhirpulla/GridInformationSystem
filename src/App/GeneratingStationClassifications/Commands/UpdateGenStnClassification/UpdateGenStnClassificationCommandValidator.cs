using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationClassifications.Commands.UpdateGenStnClassification;

public class UpdateGenStnClassificationCommandValidator : AbstractValidator<UpdateGenStnClassificationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateGenStnClassificationCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Classification)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueName(UpdateGenStnClassificationCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Regions
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}
