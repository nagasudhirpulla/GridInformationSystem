using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Buses.Queries.GetBuses;

[Authorize]
public record GetBusesQuery : IRequest<List<Bus>>;

public class GetBusesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBusesQuery, List<Bus>>
{
    public async Task<List<Bus>> Handle(GetBusesQuery request, CancellationToken cancellationToken)
    {
        var buses = await context.Buses.AsNoTracking()
                        .Include(e => e.Substation1)
                        .OrderBy(r => r.ElementNameCache)
                        .ToListAsync(cancellationToken);
        return buses;
    }
}