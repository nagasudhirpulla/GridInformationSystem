using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Owners.Queries.GetOwners;

[Authorize]
public record GetOwnersQuery : IRequest<List<Owner>>;

public class GetOwnersQueryHandler(IApplicationDbContext context) : IRequestHandler<GetOwnersQuery, List<Owner>>
{
    public async Task<List<Owner>> Handle(GetOwnersQuery request, CancellationToken cancellationToken)
    {
        var owners = await context.Owners.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return owners;
    }
}
