using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.SubFilterBanks.Queries.GetSubFilterBank;

[Authorize]
public record GetSubFilterBankQuery : IRequest<SubFilterBank>
{
    public int Id { get; init; }
}

public class GetSubFilterBankQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSubFilterBankQuery, SubFilterBank>
{
    public async Task<SubFilterBank> Handle(GetSubFilterBankQuery request, CancellationToken cancellationToken)
    {
        SubFilterBank subFilterBank = await context.SubFilterBanks.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return subFilterBank;
    }
}

