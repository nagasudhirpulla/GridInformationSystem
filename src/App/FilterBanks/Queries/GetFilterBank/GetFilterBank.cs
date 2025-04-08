using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.FilterBanks.Queries.GetFilterBank;

[Authorize]
public record GetFilterBankQuery : IRequest<FilterBank>
{
    public int Id { get; init; }
}

public class GetFilterBankQueryHandler(IApplicationDbContext context) : IRequestHandler<GetFilterBankQuery, FilterBank>
{
    public async Task<FilterBank> Handle(GetFilterBankQuery request, CancellationToken cancellationToken)
    {
        FilterBank filterBank = await context.FilterBanks.AsNoTracking()
                        .Include(e => e.ElementOwners)
                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return filterBank;
    }
}