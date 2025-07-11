using Core.Entities.Common;

namespace Core.Entities.Data;

public class Metric : AuditableEntity
{
    public required string Name { get; set; }
    public required string Unit { get; set; }
}
