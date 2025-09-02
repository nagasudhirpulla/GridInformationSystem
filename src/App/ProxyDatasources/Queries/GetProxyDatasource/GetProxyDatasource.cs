using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ProxyDatasources.Queries.GetProxyDatasource;

[Authorize]
public record GetProxyDatasourceQuery : IRequest<ProxyDatasource>
{
    public int Id { get; init; }
}

public class GetProxyDatasourceQueryHandler(IApplicationDbContext context) : IRequestHandler<GetProxyDatasourceQuery, ProxyDatasource>
{
    public async Task<ProxyDatasource> Handle(GetProxyDatasourceQuery request, CancellationToken cancellationToken)
    {
        ProxyDatasource pds = await context.ProxyDatasources.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return pds;
    }
}