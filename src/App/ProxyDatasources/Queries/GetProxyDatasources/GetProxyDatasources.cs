using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ProxyDatasources.Queries.GetProxyDatasources;

[Authorize]
public record GetProxyDatasourcesQuery : IRequest<List<ProxyDatasource>>;

public class GetProxyDatasourcesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetProxyDatasourcesQuery, List<ProxyDatasource>>
{
    public async Task<List<ProxyDatasource>> Handle(GetProxyDatasourcesQuery request, CancellationToken cancellationToken)
    {
        var items = await context.ProxyDatasources.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return items;
    }
}
