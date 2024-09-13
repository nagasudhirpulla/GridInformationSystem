using Core.Entities;

public class GeneratingStationOwners : AuditableEntity
{
    public int GeneratingStationId { get; set; }

    public required GeneratingStation GeneratingStation { get; set; }
}
