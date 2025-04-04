using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcPoles.Queries.GetHvdcPoles;

[Authorize]
public record GetHvdcPolesQuery : IRequest<List<HvdcPole>>;

public class GetHvdcPoleesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetHvdcPolesQuery, List<HvdcPole>>
{
    public async Task<List<HvdcPole>> Handle(GetHvdcPolesQuery request, CancellationToken cancellationToken)
    {
        var hvdcPoles = await context.HvdcPoles.AsNoTracking()
                        .Include(e => e.Substation1)
                        .OrderBy(r => r.ElementNameCache)
                        .ToListAsync(cancellationToken);
        return hvdcPoles;
    }
}
