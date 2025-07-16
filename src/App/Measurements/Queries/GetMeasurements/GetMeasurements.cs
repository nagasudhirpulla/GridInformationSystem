using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Measurements.Queries.GetMeasurements;

[Authorize]
public record GetMeasurementsQuery : IRequest<List<Measurement>>;

public class GetMeasurementsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMeasurementsQuery, List<Measurement>>
{
    public async Task<List<Measurement>> Handle(GetMeasurementsQuery request, CancellationToken cancellationToken)
    {
        var measurements = await context.Measurements.AsNoTracking()
                        .Include(s => s.Entity)
                        .Include(s => s.Datasource)
                        .Include(s => s.Metric)
                        .ToListAsync(cancellationToken);
        return measurements;
    }
}
