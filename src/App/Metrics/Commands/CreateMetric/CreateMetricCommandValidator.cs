using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Metrics.Commands.CreateMetric;

public class CreateMetricCommandValidator : AbstractValidator<CreateMetricCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateMetricCommandValidator(IApplicationDbContext context)
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

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Metrics
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}