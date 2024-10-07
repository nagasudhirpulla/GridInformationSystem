using Core.Entities.Common;

namespace Core.Entities;

public class VoltageLevel : AuditableEntity
{
    public required string Level { get; set; }
}