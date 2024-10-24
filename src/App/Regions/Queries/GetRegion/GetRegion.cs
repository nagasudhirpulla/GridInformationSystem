using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Regions.Queries.GetRegion;

[Authorize]
public record GetRegionQuery : IRequest<Region>
{
    public int Id { get; init; }
}

public class GetRegionQueryHandler(IApplicationDbContext context) : IRequestHandler<GetRegionQuery, Region>
{
    public async Task<Region> Handle(GetRegionQuery request, CancellationToken cancellationToken)
    {
        Region region = await context.Regions.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return region;
    }
}