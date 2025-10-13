using App.Common.Interfaces;
using Core.Entities.Elements;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace App.Lines.Utils;

public static class DeriveLineName
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
        string lineName = Execute(voltLvl, location1, location2, elementNumber);
        string region = $"{bus1.RegionCache}-{bus2.RegionCache}";
        return (lineName, voltLvl, region);
    }
    public static string Execute(string voltLvl, string location1, string location2, string elementNumber)
    {
        return $"{voltLvl}-{location1}-{location2}-{elementNumber}";
    }
    // TODO implement name change listener for voltage level, location
}