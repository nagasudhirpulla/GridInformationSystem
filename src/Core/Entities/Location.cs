namespace Core.Entities;

public class Location : GridEntity
{

    public string? Alias { get; set; }

    public required string RegionCache { get; set; }


    public State State { get; set; } = null!;
    public int StateId { get; set; }
}
