using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingUnits.Queries.GetGeneratingUnit;

[Authorize]
public record GetGeneratingUnitQuery : IRequest<GeneratingUnit>
{
    public int Id { get; init; }
}

public class GetGeneratingUnitQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGeneratingUnitQuery, GeneratingUnit>
{
    public async Task<GeneratingUnit> Handle(GetGeneratingUnitQuery request, CancellationToken cancellationToken)
    {
        GeneratingUnit genUnit = await context.GeneratingUnits.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return genUnit;
    }
}
