using App.Common.Interfaces;
using App.Common.Security;
using App.GridEntities.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GridEntities.Queries.GetGridEntities;

[Authorize]
public record GetGridEntitiesQuery : IRequest<List<GridEntityDto>>;

public class GetGridEntitiesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGridEntitiesQuery, List<GridEntityDto>>
{
    public async Task<List<GridEntityDto>> Handle(GetGridEntitiesQuery request, CancellationToken cancellationToken)
    {
        var gridElements = await context.GridEntities.AsNoTracking()
                        .Select(e => GridEntityDto.GetFromGridEntity(e))
                        .ToListAsync(cancellationToken);
        return gridElements;
    }
}
