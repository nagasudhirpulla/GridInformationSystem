using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GridEntities.Queries.GetGridEntities;

[Authorize]
public record GetGridEntitiesQuery : IRequest<List<GridEntity>>;

public class GetGridEntitiesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGridEntitiesQuery, List<GridEntity>>
{
    public async Task<List<GridEntity>> Handle(GetGridEntitiesQuery request, CancellationToken cancellationToken)
    {
        var gridElements = await context.GridEntities.AsNoTracking()
                        .ToListAsync(cancellationToken);
        return gridElements;
    }
}
