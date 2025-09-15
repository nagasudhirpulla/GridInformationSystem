using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiRoles.Queries.GetApiRoles;

[Authorize]
public record GetApiRolesQuery : IRequest<List<ApiRole>>;

public class GetApiRolesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetApiRolesQuery, List<ApiRole>>
{
    public async Task<List<ApiRole>> Handle(GetApiRolesQuery request, CancellationToken cancellationToken)
    {
        var items = await context.ApiRoles.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return items;
    }
}