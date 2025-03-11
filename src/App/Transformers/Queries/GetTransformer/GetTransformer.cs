using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Transformers.Queries.GetTransformer;

[Authorize]
public record GetTransformerQuery : IRequest<Transformer>
{
    public int Id { get; init; }
}

public class GetTransformerQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTransformerQuery, Transformer>
{
    public async Task<Transformer> Handle(GetTransformerQuery request, CancellationToken cancellationToken)
    {
        Transformer transformer = await context.Transformers.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return transformer;
    }
}
