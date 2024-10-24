using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Locations.Queries.GetLocation;

[Authorize]
public record GetLocationQuery : IRequest<Location>
{
    public int Id { get; init; }
}

public class GetLocationQueryHandler(IApplicationDbContext context) : IRequestHandler<GetLocationQuery, Location>
{
    public async Task<Location> Handle(GetLocationQuery request, CancellationToken cancellationToken)
    {
        Location location = await context.Locations.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return location;
    }
}