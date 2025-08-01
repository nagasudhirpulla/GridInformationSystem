using Core.Entities.Common;

namespace Core.Entities;

public class GridEntity : AuditableEntity
{
    public required string Name { get; set; }
}