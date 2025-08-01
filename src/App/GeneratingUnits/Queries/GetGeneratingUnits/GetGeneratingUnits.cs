using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingUnits.Queries.GetGeneratingUnits;

[Authorize]
public record GetGeneratingUnitsQuery : IRequest<List<GeneratingUnit>>;

public class GetGeneratingUnitsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGeneratingUnitsQuery, List<GeneratingUnit>>
{
    public async Task<List<GeneratingUnit>> Handle(GetGeneratingUnitsQuery request, CancellationToken cancellationToken)
    {
        var generatingUnits = await context.GeneratingUnits.AsNoTracking()
                        .Include(e => e.Substation1)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return generatingUnits;
    }
}
