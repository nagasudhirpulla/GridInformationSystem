using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Datasources.Queries.GetDatasources;

[Authorize]
public record GetDatasourcesQuery : IRequest<List<Datasource>>;

public class GetDatasourcesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetDatasourcesQuery, List<Datasource>>
{
    public async Task<List<Datasource>> Handle(GetDatasourcesQuery request, CancellationToken cancellationToken)
    {
        var datasources = await context.Datasources.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return datasources;
    }
}
