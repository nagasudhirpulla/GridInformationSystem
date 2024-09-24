namespace Core.Entities;

public class GeneratingStationOwner : AuditableEntity
{
    public required GeneratingStation GeneratingStation { get; set; }
    public int GeneratingStationId { get; set; }

    public required Owner Owner { get; set; }
    public int OwnerId { get; set; }

}
