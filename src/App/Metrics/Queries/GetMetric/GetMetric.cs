using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Metrics.Queries.GetMetric;

[Authorize]
public record GetMetricQuery : IRequest<Metric>
{
    public int Id { get; init; }
}

public class GetMetricQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMetricQuery, Metric>
{
    public async Task<Metric> Handle(GetMetricQuery request, CancellationToken cancellationToken)
    {
        Metric metric = await context.Metrics.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return metric;
    }
}