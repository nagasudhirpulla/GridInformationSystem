using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Metrics.Commands.UpdateMetric;

public class UpdateMetricCommandValidator : AbstractValidator<UpdateMetricCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMetricCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
        RuleFor(v => v.Unit)
            .NotEmpty()
            .MaximumLength(200);
    }

    public async Task<bool> BeUniqueName(UpdateMetricCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Metrics
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

}
