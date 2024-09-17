namespace Core.Entities;

public class SubstationOwner : AuditableEntity
{
    public int OwnerId { get; set; }

    public required Owner Owner { get; set; }

    public int SubstationId { get; set; }

    public required Substation Substation { get; set; }
}