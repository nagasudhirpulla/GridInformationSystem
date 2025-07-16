using Core.Entities.Common;

namespace Core.Entities.Data;

public class Datasource : AuditableEntity
{
    public required string Name { get; set; }
}
