namespace Core.Entities;

public class GeneratingStationOwner : AuditableEntity
{
    public int GeneratingStationId { get; set; }

    public required GeneratingStation GeneratingStation { get; set; }

    public int OwnerId { get; set; }

    public required Owner Owner { get; set; }
}
