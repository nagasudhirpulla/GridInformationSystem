using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcPoles.Queries.GetHvdcPole;

[Authorize]
public record GetHvdcPoleQuery : IRequest<HvdcPole>
{
    public int Id { get; init; }
}

public class GetHvdcPoleQueryHandler(IApplicationDbContext context) : IRequestHandler<GetHvdcPoleQuery, HvdcPole>
{
    public async Task<HvdcPole> Handle(GetHvdcPoleQuery request, CancellationToken cancellationToken)
    {
        HvdcPole hvdcPole = await context.HvdcPoles.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return hvdcPole;
    }
}
