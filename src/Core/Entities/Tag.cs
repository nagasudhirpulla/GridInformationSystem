using Core.Entities.Common;

namespace Core.Entities;

public class Tag : AuditableEntity
{
    public required string Name { get; set; }
    public List<GridEntityTag> GridEntityTags { get; } = [];
}
