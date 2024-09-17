namespace Core.Entities;

public class ElementOwner : AuditableEntity
{
    public int OwnerId {  get; set; }

    public required Owner Owner { get; set; }

    public int ElementId { get; set; }

    public required Element Element { get; set; }
}
