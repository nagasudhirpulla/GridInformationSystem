using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Elements.Queries.GetElements;

[Authorize]
public record GetElementsQuery : IRequest<List<Element>>;

public class GetElementsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetElementsQuery, List<Element>>
{
    public async Task<List<Element>> Handle(GetElementsQuery request, CancellationToken cancellationToken)
    {
        var elements = await context.Elements.AsNoTracking()
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return elements;
    }
}
