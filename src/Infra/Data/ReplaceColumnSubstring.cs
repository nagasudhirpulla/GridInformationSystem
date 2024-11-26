using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.Data;

public static class ReplaceColumnSubstring
{
    public static async Task ExecuteAsync(ApplicationDbContext dbContext, string oldValue, string newValue, string colName, CancellationToken cancellationToken)
    {
        // Get all entity types from the model that have columnName property of type string
        var entityTypes = dbContext.Model.GetEntityTypes()
            .Where(e => e.GetProperties()
                .Any(p => p.Name == colName &&
                         p.ClrType == typeof(string)))
            .ToList();

        if (entityTypes.Count == 0)
        {
            throw new InvalidOperationException($"No entities found with string property named '{colName}'");
        }

        foreach (var entityType in entityTypes)
        {
            // Get the CLR type for the entity
            var clrType = entityType.ClrType;

            // Verify the property type at CLR level as well
            var columnProperty = clrType.GetProperty(colName);
            if (columnProperty == null || columnProperty.PropertyType != typeof(string))
            {
                continue; // Skip if property is not found or not string type
            }

            // Get the DbSet dynamically using reflection
            var dbSetProperty = dbContext.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                   p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                   p.PropertyType.GetGenericArguments()[0] == clrType);

            if (dbSetProperty != null)
            {
                try
                {
                    // Get the DbSet instance
                    var dbSet = dbSetProperty.GetValue(dbContext);

                    // Create generic method to handle the update
                    var updateMethod = typeof(ReplaceColumnSubstring)
                        .GetMethod(nameof(UpdateColumnForType), BindingFlags.NonPublic | BindingFlags.Static)
                        .MakeGenericMethod(clrType);

                    // Invoke the generic update method
                    await (Task)updateMethod.Invoke(null, new[] { dbSet, oldValue, newValue, colName, cancellationToken });
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to update OwnersCache for entity {entityType.Name}", ex);
                }
            }
        }

        // Save all changes
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task UpdateColumnForType<TEntity>(object dbSet, string oldValue, string newValue, string colName, CancellationToken cancellationToken)
        where TEntity : class
    {
        var set = (DbSet<TEntity>)dbSet;

        // Double check the property type at runtime
        var property = typeof(TEntity).GetProperty(colName);
        if (property == null || property.PropertyType != typeof(string))
        {
            throw new InvalidOperationException($"Property {colName} of type {typeof(TEntity).Name} is not a string");
        }

        var entities = await set
            .Where(e => EF.Property<string>(e, colName).Contains(oldValue))
            .ToListAsync(cancellationToken);

        foreach (var entity in entities)
        {
            var currentValue = (string)property.GetValue(entity);
            if (currentValue != null)
            {
                property.SetValue(entity, currentValue.Replace(oldValue, newValue));
            }
        }
    }
}
