using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.Measurements.Commands.UpdateMeasurement;

public record UpdateMeasurementCommand : IRequest
{
    public int Id { get; init; }

    public int EntityId { get; init; }
    public int MetricId { get; init; }
    public int DatasourceId { get; init; }
    public required string HistorianPntId { get; init; }
}

public class UpdateMeasurementCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateMeasurementCommand>
{
    public async Task Handle(UpdateMeasurementCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Measurements
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.EntityId = request.EntityId;
        entity.MetricId = request.MetricId;
        entity.DatasourceId = request.DatasourceId;
        entity.HistorianPntId = request.HistorianPntId;
        await context.SaveChangesAsync(cancellationToken);
    }
}

