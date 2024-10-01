using Core.Entities.Common;

namespace Core.Entities;

public class Location : AuditableEntity
{
    public required string Name { get; set; }

    public string? Alias { get; set; }

    public Region Region { get; set; } = null!;
    public int RegionId { get; set; }


    public State State { get; set; } = null!;
    public int StateId { get; set; }
}
