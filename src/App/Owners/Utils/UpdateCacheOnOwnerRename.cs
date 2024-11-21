using App.Common.Interfaces;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Owners.Utils;

public class UpdateCacheOnOwnerRename
{
    public static async Task ExecuteAsync(string existingOwnerName, string newOwnerName, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        // find all entities which have the property "OwnerNamesCache"
        var entityTypes = context.GetType()
                            .GetProperties()
                            .Where(prop => prop.PropertyType.GetProperties()
                                .Any(p => p.Name.Equals("OwnerNamesCache", StringComparison.OrdinalIgnoreCase)))
                            .Select(prop => prop.PropertyType)
                            .ToList() ?? [];

        // Search for "existingOwnerName;" and replace with "newOwnerName;" in all the entitites
    }
}
