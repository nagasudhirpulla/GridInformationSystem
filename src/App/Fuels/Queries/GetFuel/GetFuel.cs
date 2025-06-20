using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Fuels.Queries.GetFuel;

[Authorize]
public record GetFuelQuery : IRequest<Fuel>
{
    public int Id { get; init; }
}

public class GetFuelQueryHandler(IApplicationDbContext context) : IRequestHandler<GetFuelQuery, Fuel>
{
    public async Task<Fuel> Handle(GetFuelQuery request, CancellationToken cancellationToken)
    {
        Fuel fuel = await context.Fuels.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return fuel;
    }
}