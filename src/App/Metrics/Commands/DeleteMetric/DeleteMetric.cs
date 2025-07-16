using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Metrics.Commands.DeleteMetric;

public record DeleteMetricCommand(int Id) : IRequest;

public class DeleteMetricCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteMetricCommand>
{
    public async Task Handle(DeleteMetricCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Metrics
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Metrics.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
