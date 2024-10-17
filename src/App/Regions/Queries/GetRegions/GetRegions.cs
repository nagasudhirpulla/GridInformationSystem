using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Regions.Queries.GetRegions;

[Authorize]
public record GetRegionsQuery : IRequest<List<Region>>;

public class GetRegionsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetRegionsQuery, List<Region>>
{
    public async Task<List<Region>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
    {
        var regions = await context.Regions.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return regions;
    }
}
