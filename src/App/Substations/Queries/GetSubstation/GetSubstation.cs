using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Queries.GetSubstation;

[Authorize]
public record GetSubstationQuery : IRequest<Substation>
{
    public int Id { get; init; }
}

public class GetSubstationQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSubstationQuery, Substation>
{
    public async Task<Substation> Handle(GetSubstationQuery request, CancellationToken cancellationToken)
    {
        Substation substation = await context.Substations.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return substation;
    }
}