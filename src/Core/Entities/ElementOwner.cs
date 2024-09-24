using Core.Entities.Elements;

namespace Core.Entities;

public class ElementOwner : AuditableEntity
{
    public required Owner Owner { get; set; }
    public int OwnerId { get; set; }

    public required Element Element { get; set; }
    public int ElementId { get; set; }
}
