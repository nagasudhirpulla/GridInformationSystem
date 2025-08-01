using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Lines.Queries.GetLines;

[Authorize]
public record GetLinesQuery : IRequest<List<Line>>;

public class GetLinesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetLinesQuery, List<Line>>
{
    public async Task<List<Line>> Handle(GetLinesQuery request, CancellationToken cancellationToken)
    {
        var lines = await context.Lines.AsNoTracking()
                        .Include(e => e.Substation1)
                        .Include(e => e.Substation2)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return lines;
    }
}