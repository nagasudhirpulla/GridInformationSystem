using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Metrics.Queries.GetMetrics;

[Authorize]
public record GetMetricsQuery : IRequest<List<Metric>>;

public class GetMetricsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMetricsQuery, List<Metric>>
{
    public async Task<List<Metric>> Handle(GetMetricsQuery request, CancellationToken cancellationToken)
    {
        var metrics = await context.Metrics.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return metrics;
    }
}
