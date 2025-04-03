using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcLines.Queries.GetHvdcLines;

[Authorize]
public record GetHvdcLinesQuery : IRequest<List<HvdcLine>>;

public class GetHvdcLinesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetHvdcLinesQuery, List<HvdcLine>>
{
    public async Task<List<HvdcLine>> Handle(GetHvdcLinesQuery request, CancellationToken cancellationToken)
    {
        var lines = await context.HvdcLines.AsNoTracking()
                        .Include(e => e.Substation1)
                        .Include(e => e.Substation2)
                        .OrderBy(r => r.ElementNameCache)
                        .ToListAsync(cancellationToken);
        return lines;
    }
}
