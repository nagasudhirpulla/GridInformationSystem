using Core.Entities.Common;

namespace Core.Entities;

public class Location : AuditableEntity
{
    public required string Name { get; set; }

    public string? Alias { get; set; }

    public required string RegionCache { get; set; }


    public State State { get; set; } = null!;
    public int StateId { get; set; }
}
