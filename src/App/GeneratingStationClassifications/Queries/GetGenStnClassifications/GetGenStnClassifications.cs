using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationClassifications.Queries.GetGenStnClassifications;

[Authorize]
public record GetGenStnClassificationsQuery : IRequest<List<GeneratingStationClassification>>;

public class GetGenStnClassificationsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGenStnClassificationsQuery, List<GeneratingStationClassification>>
{
    public async Task<List<GeneratingStationClassification>> Handle(GetGenStnClassificationsQuery request, CancellationToken cancellationToken)
    {
        var genStnClassifications = await context.GeneratingStationClassifications.AsNoTracking()
                        .OrderBy(r => r.Classification)
                        .ToListAsync(cancellationToken);
        return genStnClassifications;
    }
}
