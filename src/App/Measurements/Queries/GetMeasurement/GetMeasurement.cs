using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Measurements.Queries.GetMeasurement;

[Authorize]
public record GetMeasurementQuery : IRequest<Measurement>
{
    public int Id { get; init; }
}

public class GetMeasurementQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMeasurementQuery, Measurement>
{
    public async Task<Measurement> Handle(GetMeasurementQuery request, CancellationToken cancellationToken)
    {
        Measurement measurement = await context.Measurements
                        .Include(e => e.Metric)
                        .Include(e => e.Datasource)
                        .Include(e => e.Entity)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return measurement;
    }
}