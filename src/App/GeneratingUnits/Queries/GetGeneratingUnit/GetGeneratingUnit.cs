using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingUnits.Queries.GetGeneratingUnit;

[Authorize]
public record GetGeneratingUnitQuery : IRequest<Bus>
{
    public int Id { get; init; }
}

public class GetGeneratingUnitQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGeneratingUnitQuery, Bus>
{
    public async Task<Bus> Handle(GetGeneratingUnitQuery request, CancellationToken cancellationToken)
    {
        Bus bus = await context.Buses.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return bus;
    }
}
