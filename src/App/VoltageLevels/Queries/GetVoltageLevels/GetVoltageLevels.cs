using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.VoltageLevels.Queries.GetVoltageLevels;

[Authorize]
public record GetVoltageLevelsQuery : IRequest<List<VoltageLevel>>;

public class GetVoltageLevelsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetVoltageLevelsQuery, List<VoltageLevel>>
{
    public async Task<List<VoltageLevel>> Handle(GetVoltageLevelsQuery request, CancellationToken cancellationToken)
    {
        var regions = await context.VoltageLevels.AsNoTracking()
                        .OrderBy(r => r.Level)
                        .ToListAsync(cancellationToken);
        return regions;
    }
}