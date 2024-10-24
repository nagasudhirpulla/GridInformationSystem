using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Locations.Queries.GetLocations;

[Authorize]
public record GetLocationsQuery : IRequest<List<Location>>;

public class GetLocationsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetLocationsQuery, List<Location>>
{
    public async Task<List<Location>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
    {
        var locations = await context.Locations.AsNoTracking()
                        .Include(s => s.State)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return locations;
    }
}