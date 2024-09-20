namespace Core.Entities;

/*
combination of FromBus, ToBus and CircuitNo is unique
Bus1 and Bus2 should belong to AC Substation and both the substations should be different with the same voltage level
*/
public class Line : AuditableEntity
{
    public required Bus Bus1 { get; set; }
    public int Bus1Id { get; set; }

    public required Bus Bus2 { get; set; }
    public int Bus2Id { get; set; }
    public double Length { get; set; }
    public required string ConductorType { get; set; }
    public bool IsAutoReclosurePresent { get; set; }
}
