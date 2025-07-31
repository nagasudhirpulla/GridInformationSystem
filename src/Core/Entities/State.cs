namespace Core.Entities;

public class State : GridEntity
{
    public required string Name { get; set; }

    public Region Region { get; set; } = null!;
    public int RegionId { get; set; }
}
