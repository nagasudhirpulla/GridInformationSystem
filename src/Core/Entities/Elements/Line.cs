namespace Core.Entities.Elements;

/*
combination of FromBus, ToBus and Element number is unique
Bus1 and Bus2 should belong to AC Substation and both the substations should be different with the same voltage level
*/
public class Line : Element
{
    public Bus Bus1 { get; set; } = null!;
    public int Bus1Id { get; set; }

    public Bus Bus2 { get; set; } = null!;
    public int Bus2Id { get; set; }

    public double Length { get; set; }

    public required string ConductorType { get; set; }

    public bool IsAutoReclosurePresent { get; set; }
}
