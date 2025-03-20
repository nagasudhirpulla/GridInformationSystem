using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.LineReactors.Queries.GetLineReactor;

[Authorize]
public record GetLineReactorQuery : IRequest<LineReactor>
{
    public int Id { get; init; }
}

public class GetLineReactorQueryHandler(IApplicationDbContext context) : IRequestHandler<GetLineReactorQuery, LineReactor>
{
    public async Task<LineReactor> Handle(GetLineReactorQuery request, CancellationToken cancellationToken)
    {
        LineReactor lineReactor = await context.LineReactors.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return lineReactor;
    }
}
