using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationClassifications.Queries.GetGenStnClassification;

[Authorize]
public record GetGenStnClassificationQuery : IRequest<GeneratingStationClassification>
{
    public int Id { get; init; }
}

public class GetGenStnClassificationQueryHandler(IApplicationDbContext context) : IRequestHandler<GetGenStnClassificationQuery, GeneratingStationClassification>
{
    public async Task<GeneratingStationClassification> Handle(GetGenStnClassificationQuery request, CancellationToken cancellationToken)
    {
        GeneratingStationClassification region = await context.GeneratingStationClassifications.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return region;
    }
}
