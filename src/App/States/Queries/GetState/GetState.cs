using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.States.Queries.GetState;

[Authorize]
public record GetStateQuery : IRequest<State>
{
    public int Id { get; init; }
}

public class GetStateQueryHandler(IApplicationDbContext context) : IRequestHandler<GetStateQuery, State>
{
    public async Task<State> Handle(GetStateQuery request, CancellationToken cancellationToken)
    {
        State state = await context.States.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return state;
    }
}