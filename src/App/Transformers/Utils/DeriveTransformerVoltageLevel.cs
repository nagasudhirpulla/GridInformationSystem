using App.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace App.Transformers.Utils;

public static class DeriveTransformerVoltageLevel
{
    public static string Execute(string hvLevel, string lvLevel)
    {
        return $"{hvLevel}/{lvLevel}";
    }

    public static async Task<string> FromEntity(int tId, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        var t = await context.Transformers
                        .Include(t => t.Substation1)
                        .ThenInclude(t=>t.VoltageLevel)
                        .Include(t => t.Substation2)
                        .ThenInclude(t=>t.VoltageLevel)
                        .Where(t => t.Id == tId)
                        .FirstOrDefaultAsync(cancellationToken);
        IOrderedEnumerable<string> hvLvLevels = new[] { t.Substation1.VoltageLevel.Level, t.Substation2.VoltageLevel.Level }.OrderByDescending(i => int.Parse(Regex.Match(i, @"\d+").Value));
        return Execute(hvLvLevels.ElementAt(0), hvLvLevels.ElementAt(1));
    }
}
