using App.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Measurements.Commands.UpdateMeasurement;

public class UpdateMeasurementCommandValidator : AbstractValidator<UpdateMeasurementCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMeasurementCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.HistorianPntId)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(v => v.EntityId)
            .MustAsync(async (int sId, CancellationToken cancellationToken) =>
            {
                return await _context.GridEntities.AnyAsync(s => s.Id == sId, cancellationToken);
            })
                .WithMessage("'{PropertyName}' does not exist.")
                .WithErrorCode("NotExists");

        RuleFor(v => v.MetricId)
            .MustAsync(async (int sId, CancellationToken cancellationToken) =>
            {
                return await _context.Metrics.AnyAsync(s => s.Id == sId, cancellationToken);
            })
                .WithMessage("'{PropertyName}' does not exist.")
                .WithErrorCode("NotExists");

        RuleFor(v => v.DatasourceId)
            .MustAsync(async (int sId, CancellationToken cancellationToken) =>
            {
                return await _context.Datasources.AnyAsync(s => s.Id == sId, cancellationToken);
            })
                .WithMessage("'{PropertyName}' does not exist.")
                .WithErrorCode("NotExists");
    }
}

