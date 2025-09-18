using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiClients.Queries.GetApiClients;

[Authorize]
public record GetApiClientsQuery : IRequest<List<ApiClient>>;

public class GetApiClientsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetApiClientsQuery, List<ApiClient>>
{
    public async Task<List<ApiClient>> Handle(GetApiClientsQuery request, CancellationToken cancellationToken)
    {
        var apiClients = await context.ApiClients.AsNoTracking()
            .Include(s => s.ApiClientRoles)
            .ThenInclude(ar => ar.ApiRole)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
        return apiClients;
    }
}
