using App.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiClients.Queries.CheckApiClientRole;

public record CheckApiClientRoleQuery : IRequest<bool>
{
    public required string Name { get; init; }
    public required string Role { get; set; }
}

public class CheckApiClientRoleQueryHandler(IApplicationDbContext context) : IRequestHandler<CheckApiClientRoleQuery, bool>
{
    public async Task<bool> Handle(CheckApiClientRoleQuery request, CancellationToken cancellationToken)
    {
        bool isClientInRole = await context.ApiClientRoles.AsNoTracking()
                                    .AnyAsync(s => s.ApiClient.Name == request.Name &&
                                                s.ApiRole.Name.ToLower().Trim() == request.Role.ToLower().Trim(),
                                                    cancellationToken: cancellationToken);
        return isClientInRole;
    }
}