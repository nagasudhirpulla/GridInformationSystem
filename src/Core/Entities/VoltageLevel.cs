using Core.Entities.Common;

namespace Core.Entities;

public class VoltageLevel : AuditableEntity
{
    public int Level { get; set; }
}