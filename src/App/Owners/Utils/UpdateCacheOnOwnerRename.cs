using App.Common.Interfaces;
using Core.Entities.Elements;

namespace App.Owners.Utils;

public class UpdateCacheOnOwnerRename
{
    public static async Task ExecuteAsync(string existingOwnerName, string newOwnerName, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        await context.ReplaceSubstringInColumn($"{existingOwnerName};", $"{newOwnerName};", nameof(Element.OwnerNamesCache), cancellationToken);
    }
}
