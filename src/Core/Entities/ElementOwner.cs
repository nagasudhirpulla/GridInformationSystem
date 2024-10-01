using Core.Entities.Common;
using Core.Entities.Elements;

namespace Core.Entities;

public class ElementOwner : AuditableEntity
{
    public Owner Owner { get; set; } = null!;
    public int OwnerId { get; set; }

    public Element Element { get; set; } = null!;
    public int ElementId { get; set; }
}
