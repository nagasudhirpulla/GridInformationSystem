using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiRoles.Queries.GetApiRole;

[Authorize]
public record GetApiRoleQuery : IRequest<ApiRole>
{
    public int Id { get; init; }
}

public class GetApiRoleQueryHandler(IApplicationDbContext context) : IRequestHandler<GetApiRoleQuery, ApiRole>
{
    public async Task<ApiRole> Handle(GetApiRoleQuery request, CancellationToken cancellationToken)
    {
        ApiRole item = await context.ApiRoles.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return item;
    }
}