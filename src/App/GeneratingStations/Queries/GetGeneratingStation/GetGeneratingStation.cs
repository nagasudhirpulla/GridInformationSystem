using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStations.Queries.GetGeneratingStation;

[Authorize]
public record GetGeneratingStationQuery : IRequest<GeneratingStation>
{
    public int Id { get; init; }
}

public class GetGeneratingStationQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGeneratingStationQuery, GeneratingStation>
{
    public async Task<GeneratingStation> Handle(GetGeneratingStationQuery request, CancellationToken cancellationToken)
    {
        GeneratingStation genStation = await context.GeneratingStations.AsNoTracking()
                        .Include(x => x.SubstationOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return genStation;
    }
}
