using App.Common.Interfaces;
using Core.Entities.Elements;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcLines.Utils;

public static class DeriveHvdcLineName
{
    public static async Task<(string lineName, string voltLevel, string region)> ExecuteAsync(int bus1Id, int bus2Id, string elementNumber, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        Bus bus1 = await context.Buses
                .AsNoTracking()
                .Include(e => e.Substation1)
                .ThenInclude(s => s.Location).FirstOrDefaultAsync(e => e.Id == bus1Id, cancellationToken: cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation Id is not present in database"
                                                                                }]);
        Bus bus2 = await context.Buses
                .AsNoTracking()
                .Include(e => e.Substation1)
                .ThenInclude(s => s.Location).FirstOrDefaultAsync(e => e.Id == bus2Id, cancellationToken: cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation Id is not present in database"
                                                                                }]);
        string voltLvl = bus1.VoltageLevelCache;
        string location1 = bus1.Substation1.Location.Name;
        string location2 = bus2.Substation1.Location.Name;
        string lineName = $"HVDC {voltLvl}-{location1}-{location2}-{elementNumber}";
        string region = $"{bus1.RegionCache}-{bus2.RegionCache}";
        return (lineName, voltLvl, region);
    }

    public static async Task<string> FromEntity(int hId, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        var hl = await context.HvdcLines
            .Include(e => e.Substation1).ThenInclude(e => e.VoltageLevel)
            .Include(e => e.Substation1).ThenInclude(e => e.Location)
            .Include(e => e.Substation2).ThenInclude(e => e.Location)
            .Where(e => e.Id == hId)
            .FirstOrDefaultAsync(cancellationToken)??throw new KeyNotFoundException();
        string lineName = $"HVDC {hl.Substation1.VoltageLevel.Level}-{hl.Substation1.Location.Name}-{hl.Substation2?.Location.Name}-{hl.ElementNumber}";
        return lineName;
    }
}
