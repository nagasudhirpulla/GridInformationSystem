using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.SubFilterBanks.Queries.GetSubFilterBanks;

[Authorize]
public record GetSubFilterBanksQuery : IRequest<List<SubFilterBank>>;

public class GetSubFilterBankesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSubFilterBanksQuery, List<SubFilterBank>>
{
    public async Task<List<SubFilterBank>> Handle(GetSubFilterBanksQuery request, CancellationToken cancellationToken)
    {
        var subFilterBanks = await context.SubFilterBanks.AsNoTracking()
                        .Include(e => e.Substation1)
                        .OrderBy(r => r.ElementNameCache)
                        .ToListAsync(cancellationToken);
        return subFilterBanks;
    }
}
