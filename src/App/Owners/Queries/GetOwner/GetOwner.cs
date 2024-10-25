using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Owners.Queries.GetOwner;

[Authorize]
public record GetOwnerQuery : IRequest<Owner>
{
    public int Id { get; init; }
}

public class GetOwnerQueryHandler(IApplicationDbContext context) : IRequestHandler<GetOwnerQuery, Owner>
{
    public async Task<Owner> Handle(GetOwnerQuery request, CancellationToken cancellationToken)
    {
        Owner owner = await context.Owners.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return owner;
    }
}