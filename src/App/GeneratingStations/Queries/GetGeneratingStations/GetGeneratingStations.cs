using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStations.Queries.GetGeneratingStations;

[Authorize]
public record GetGeneratingStationsQuery : IRequest<List<GeneratingStation>>;

public class GetGeneratingStationsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGeneratingStationsQuery, List<GeneratingStation>>
{
    public async Task<List<GeneratingStation>> Handle(GetGeneratingStationsQuery request, CancellationToken cancellationToken)
    {
        var substations = await context.GeneratingStations.AsNoTracking()
            .Include(s => s.VoltageLevel)
            .Include(s => s.Location)
            .Include(s => s.GeneratingStationType)
            .Include(s => s.GeneratingStationClassification)
            .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return substations;
    }
}
