using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Bays.Queries.GetBays;

[Authorize]
public record GetBaysQuery : IRequest<List<Bay>>;

public class GetBaysQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBaysQuery, List<Bay>>
{
    public async Task<List<Bay>> Handle(GetBaysQuery request, CancellationToken cancellationToken)
    {
        var bays = await context.Bays.AsNoTracking()
                        .Include(e => e.Substation1)
                        .Include(e => e.Element1)
                        .Include(e => e.Element2)
                        .OrderBy(r => r.ElementNameCache)
                        .ToListAsync(cancellationToken);
        return bays;
    }
}
