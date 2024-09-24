using Core.Entities.Common;

namespace Core.Entities;

public class Location : AuditableEntity
{
    public required string Name { get; set; }

    public string? Alias { get; set; }

    public required Region Region { get; set; }
    public int RegionId { get; set; }


    public required State State { get; set; }
    public int StateId { get; set; }
}
