using App.Common.Interfaces;
using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;

namespace App.Buses.Utils;

public static class IsAnAcBus
{
    public static async Task<bool> ExecuteAsync(int busId, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        Bus bus = await context.Buses.AsNoTracking()
                        .Include(e => e.Substation1)
                        .FirstOrDefaultAsync(s => s.Id == busId, cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        return bus.Substation1.IsAc;
    }
}