using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.LineReactors.Queries.GetLineReactors;

[Authorize]
public record GetLineReactorsQuery : IRequest<List<LineReactor>>;

public class GetLineReactorsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetLineReactorsQuery, List<LineReactor>>
{
    public async Task<List<LineReactor>> Handle(GetLineReactorsQuery request, CancellationToken cancellationToken)
    {
        var lines = await context.LineReactors.AsNoTracking()
                        .Include(e => e.Substation1)
                        .Include(e => e.Line)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return lines;
    }
}
