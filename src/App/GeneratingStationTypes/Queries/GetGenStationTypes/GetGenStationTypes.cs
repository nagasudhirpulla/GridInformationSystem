using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationTypes.Queries.GetGenStationTypes;

[Authorize]
public record GetGenStationTypesQuery : IRequest<List<GeneratingStationType>>;

public class GetGenStationTypesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGenStationTypesQuery, List<GeneratingStationType>>
{
    public async Task<List<GeneratingStationType>> Handle(GetGenStationTypesQuery request, CancellationToken cancellationToken)
    {
        var genStnTypes = await context.GeneratingStationTypes.AsNoTracking()
                        .OrderBy(r => r.StationType)
                        .ToListAsync(cancellationToken);
        return genStnTypes;
    }
}