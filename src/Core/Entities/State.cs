using Core.Entities.Common;

namespace Core.Entities;

public class State : AuditableEntity
{
    public required string Name { get; set; }

    public required Region Region { get; set; }
    public int RegionId { get; set; }
}
