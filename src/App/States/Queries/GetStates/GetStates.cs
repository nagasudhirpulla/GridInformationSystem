using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.States.Queries.GetStates;

[Authorize]
public record GetStatesQuery : IRequest<List<State>>;

public class GetStatesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetStatesQuery, List<State>>
{
    public async Task<List<State>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
    {
        var states = await context.States.AsNoTracking()
                        .Include(s=>s.Region)
                        .OrderBy(r => r.Name)
                        .ToListAsync(cancellationToken);
        return states;
    }
}