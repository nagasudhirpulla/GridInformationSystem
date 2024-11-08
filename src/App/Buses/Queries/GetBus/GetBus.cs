using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Buses.Queries.GetBus;

[Authorize]
public record GetBusQuery : IRequest<Bus>
{
    public int Id { get; init; }
}

public class GetBusQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBusQuery, Bus>
{
    public async Task<Bus> Handle(GetBusQuery request, CancellationToken cancellationToken)
    {
        Bus bus = await context.Buses.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return bus;
    }
}