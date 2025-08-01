using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Queries.GetSubstations;

[Authorize]
public record GetSubstationsQuery : IRequest<List<Substation>>;

public class GetSubstationsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSubstationsQuery, List<Substation>>
{
    public async Task<List<Substation>> Handle(GetSubstationsQuery request, CancellationToken cancellationToken)
    {
        var substations = await context.Substations.AsNoTracking()
            .Include(s => s.VoltageLevel)
            .Include(s => s.Location)
            .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return substations;
    }
}