using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Bays.Queries.GetBay;

[Authorize]
public record GetBayQuery : IRequest<Bay>
{
    public int Id { get; init; }
}

public class GetBayQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBayQuery, Bay>
{
    public async Task<Bay> Handle(GetBayQuery request, CancellationToken cancellationToken)
    {
        Bay bay = await context.Bays.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return bay;
    }
}
