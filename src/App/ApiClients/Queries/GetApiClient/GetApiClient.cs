using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiClients.Queries.GetApiClient;

[Authorize]
public record GetApiClientQuery : IRequest<ApiClient>
{
    public int Id { get; init; }
}

public class GetApiClientQueryHandler(IApplicationDbContext context) : IRequestHandler<GetApiClientQuery, ApiClient>
{
    public async Task<ApiClient> Handle(GetApiClientQuery request, CancellationToken cancellationToken)
    {
        ApiClient apiClient = await context.ApiClients.AsNoTracking()
                        .Include(x => x.ApiClientRoles)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return apiClient;
    }
}