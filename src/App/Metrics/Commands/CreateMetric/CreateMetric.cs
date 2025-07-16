using App.Common.Interfaces;
using Core.Entities.Data;
using MediatR;

namespace App.Metrics.Commands.CreateMetric;

public record CreateMetricCommand : IRequest<int>
{
    public required string Name { get; init; }
    public required string Unit { get; init; }
}

public class CreateMetricCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateMetricCommand, int>
{
    public async Task<int> Handle(CreateMetricCommand request, CancellationToken cancellationToken)
    {
        var entity = new Metric() { Name = request.Name, Unit = request.Unit };

        context.Metrics.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
