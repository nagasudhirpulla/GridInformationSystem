using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Fuels.Queries.GetFuels;

[Authorize]
public record GetFuelsQuery : IRequest<List<Fuel>>;

public class GetFuelsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetFuelsQuery, List<Fuel>>
{
    public async Task<List<Fuel>> Handle(GetFuelsQuery request, CancellationToken cancellationToken)
    {
        var fuels = await context.Fuels.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return fuels;
    }
}