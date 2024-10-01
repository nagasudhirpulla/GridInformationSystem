namespace Core.Entities.Elements;

/*
combination of FromBus, ToBus and CircuitNo is unique
FromBus and ToBus should belong to HVDC Substation and both the substations should be different with the same voltage level
*/
public class HvdcLine : Element
{
    public Bus Bus1 { get; set; } = null!;
    public int Bus1Id { get; set; }

    public Bus Bus2 { get; set; } = null!;
    public int Bus2Id { get; set; }

    public double Length { get; set; }

    public required string ConductorType { get; set; }

    public bool IsSpsPresent { get; set; }
}
