namespace Core.Entities;

public class State : GridEntity
{

    public Region Region { get; set; } = null!;
    public int RegionId { get; set; }
}
