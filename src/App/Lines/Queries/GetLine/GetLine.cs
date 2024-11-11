using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Lines.Queries.GetLine;

[Authorize]
public record GetLineQuery : IRequest<Line>
{
    public int Id { get; init; }
}

public class GetLineQueryHandler(IApplicationDbContext context) : IRequestHandler<GetLineQuery, Line>
{
    public async Task<Line> Handle(GetLineQuery request, CancellationToken cancellationToken)
    {
        Line line = await context.Lines.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return line;
    }
}