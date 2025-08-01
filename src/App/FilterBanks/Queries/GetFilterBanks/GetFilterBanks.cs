using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.FilterBanks.Queries.GetFilterBanks;

[Authorize]
public record GetFilterBanksQuery : IRequest<List<FilterBank>>;

public class GetFilterBanksQueryHandler(IApplicationDbContext context) : IRequestHandler<GetFilterBanksQuery, List<FilterBank>>
{
    public async Task<List<FilterBank>> Handle(GetFilterBanksQuery request, CancellationToken cancellationToken)
    {
        var filterBanks = await context.FilterBanks.AsNoTracking()
                        .Include(e => e.Substation1)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return filterBanks;
    }
}
