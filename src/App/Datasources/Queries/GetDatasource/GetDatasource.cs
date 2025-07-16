using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Datasources.Queries.GetDatasource;

[Authorize]
public record GetDatasourceQuery : IRequest<Datasource>
{
    public int Id { get; init; }
}

public class GetDatasourceQueryHandler(IApplicationDbContext context) : IRequestHandler<GetDatasourceQuery, Datasource>
{
    public async Task<Datasource> Handle(GetDatasourceQuery request, CancellationToken cancellationToken)
    {
        Datasource region = await context.Datasources.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return region;
    }
}