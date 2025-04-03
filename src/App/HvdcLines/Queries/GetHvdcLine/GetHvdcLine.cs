using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcLines.Queries.GetHvdcLine;

[Authorize]
public record GetHvdcLineQuery : IRequest<HvdcLine>
{
    public int Id { get; init; }
}

public class GetHvdcLineQueryHandler(IApplicationDbContext context) : IRequestHandler<GetHvdcLineQuery, HvdcLine>
{
    public async Task<HvdcLine> Handle(GetHvdcLineQuery request, CancellationToken cancellationToken)
    {
        HvdcLine line = await context.HvdcLines.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return line;
    }
}
