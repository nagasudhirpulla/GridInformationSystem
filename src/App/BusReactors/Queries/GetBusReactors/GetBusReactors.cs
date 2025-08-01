using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.BusReactors.Queries.GetBusReactors;

[Authorize]
public record GetBusReactorsQuery : IRequest<List<BusReactor>>;

public class GetBusReactorsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBusReactorsQuery, List<BusReactor>>
{
    public async Task<List<BusReactor>> Handle(GetBusReactorsQuery request, CancellationToken cancellationToken)
    {
        var busReactors = await context.BusReactors.AsNoTracking()
                        .Include(e => e.Substation1)
                        .Include(e => e.Bus)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return busReactors;
    }
}