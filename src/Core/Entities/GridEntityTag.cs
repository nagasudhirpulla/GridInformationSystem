using Core.Entities.Common;

namespace Core.Entities;

public class GridEntityTag : AuditableEntity
{
    public GridEntity GridEntity { get; set; } = null!;
    public int GridEntityId { get; set; }

    public Tag Tag { get; set; } = null!;
    public int TagId { get; set; }
}
