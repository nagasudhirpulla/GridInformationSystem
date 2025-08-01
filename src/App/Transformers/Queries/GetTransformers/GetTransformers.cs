using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Transformers.Queries.GetTransformers;

[Authorize]
public record GetTransformersQuery : IRequest<List<Transformer>>;

public class GetTransformersQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTransformersQuery, List<Transformer>>
{
    public async Task<List<Transformer>> Handle(GetTransformersQuery request, CancellationToken cancellationToken)
    {
        var transformers = await context.Transformers.AsNoTracking()
                        .Include(e => e.Substation1)
                        .Include(e => e.Substation2)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return transformers;
    }
}
