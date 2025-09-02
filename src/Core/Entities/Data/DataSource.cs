using Core.Entities.Common;

namespace Core.Entities.Data;

/// <summary>
/// Represents a Datasource where data would be stored locally
/// </summary>
public class Datasource : AuditableEntity
{
    public required string Name { get; set; }
}
