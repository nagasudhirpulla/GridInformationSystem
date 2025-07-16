using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.Metrics.Commands.UpdateMetric;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateMetricCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
    public required string Unit { get; init; }
}

public class UpdateMetricCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateMetricCommand>
{
    public async Task Handle(UpdateMetricCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Metrics
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        var existingMetricName = entity.Name;

        // update entity attributes
        entity.Name = request.Name;
        entity.Unit = request.Unit;

        await context.SaveChangesAsync(cancellationToken);
    }
}
