using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.BusReactors.Queries.GetBusReactor;

[Authorize]
public record GetBusReactorQuery : IRequest<BusReactor>
{
    public int Id { get; init; }
}

public class GetBusReactorQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBusReactorQuery, BusReactor>
{
    public async Task<BusReactor> Handle(GetBusReactorQuery request, CancellationToken cancellationToken)
    {
        BusReactor busReactor = await context.BusReactors.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return busReactor;
    }
}
