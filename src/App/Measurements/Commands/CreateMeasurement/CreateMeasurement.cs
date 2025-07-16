using App.Common.Interfaces;
using Core.Entities.Data;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Measurements.Commands.CreateMeasurement;

public record CreateMeasurementCommand : IRequest<int>
{
    public int EntityId { get; init; }
    public int MetricId { get; init; }
    public int DatasourceId { get; init; }
    public required string HistorianPntId { get; init; }
}

public class CreateMeasurementCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateMeasurementCommand, int>
{
    public async Task<int> Handle(CreateMeasurementCommand request, CancellationToken cancellationToken)
    {
        _ = await context.Metrics
                    .Where(s => s.Id == request.MetricId)
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                    ErrorMessage = "Metric Id is not present in database"
                                                                }]);
        _ = await context.Datasources
                    .Where(s => s.Id == request.DatasourceId)
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                    ErrorMessage = "Datasource Id is not present in database"
                                                                }]);
        _ = await context.GridEntities
                    .Where(s => s.Id == request.EntityId)
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                    ErrorMessage = "GridEntity Id is not present in database"
                                                                }]);
        var entity = new Measurement()
        {
            EntityId = request.EntityId,
            MetricId = request.MetricId,
            DatasourceId = request.DatasourceId,
            HistorianPntId = request.HistorianPntId
        };

        context.Measurements.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
