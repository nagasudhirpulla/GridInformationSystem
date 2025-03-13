using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationTypes.Queries.GetGenStationType;

[Authorize]
public record GetGenStationTypeQuery : IRequest<GeneratingStationType>
{
    public int Id { get; init; }
}

public class GetGenStationTypeQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGenStationTypeQuery, GeneratingStationType>
{
    public async Task<GeneratingStationType> Handle(GetGenStationTypeQuery request, CancellationToken cancellationToken)
    {
        GeneratingStationType genStnType = await context.GeneratingStationTypes.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return genStnType;
    }
}
