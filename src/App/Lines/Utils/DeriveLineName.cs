using App.Common.Interfaces;
using Core.Entities.Elements;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        string voltLvl = bus1.VoltLevelCache;
        string location1 = bus1.Substation1.Location.Name;
        string location2 = bus2.Substation1.Location.Name;
        string lineName = $"{voltLvl}-{location1}-{location2}-{elementNumber}";
        string region = $"{bus1.RegionCache}-{bus2.RegionCache}";
        return (lineName, voltLvl, region);
    }
}