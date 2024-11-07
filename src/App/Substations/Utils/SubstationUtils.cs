using App.Common.Interfaces;
using Core.Entities;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Utils;
public static class SubstationUtils
{
    public static string DeriveSubstationName(string voltageLevel, string location)
    {
        return $"{voltageLevel} {location}";
    }

    public static async Task<string> DeriveSubstationName(int voltageLevelId, int locationId, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        Location location = await context.Locations.FirstOrDefaultAsync(l => l.Id == locationId, cancellationToken)
                                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Location Id is not present in database"
                                                                                }]);
        VoltageLevel voltageLevel = await context.VoltageLevels.FirstOrDefaultAsync(l => l.Id == voltageLevelId, cancellationToken)
                                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Voltage level Id is not present in database"
                                                                                }]);
        return DeriveSubstationName(voltageLevel.Level, location.Name);
    }

    public static async Task<bool> IsElementsConnected(int substationId, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        bool isElementsConnected = await context.Elements.AnyAsync(e => (e.Substation1Id == substationId) || (e.Substation2Id == substationId), cancellationToken);
        return isElementsConnected;
    }
}