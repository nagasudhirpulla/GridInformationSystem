using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.VoltageLevels.Queries.GetVoltageLevel;

[Authorize]
public record GetVoltageLevelQuery : IRequest<VoltageLevel>
{
    public int Id { get; init; }
}

public class GetVoltageLevelQueryHandler(IApplicationDbContext context) : IRequestHandler<GetVoltageLevelQuery, VoltageLevel>
{
    public async Task<VoltageLevel> Handle(GetVoltageLevelQuery request, CancellationToken cancellationToken)
    {
        VoltageLevel voltLvl = await context.VoltageLevels.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return voltLvl;
    }
}