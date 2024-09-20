namespace Core.Entities;

/*
combination of FromBus, ToBus and CircuitNo is unique
FromBus and ToBus should belong to HVDC Substation and both the substations should be different with the same voltage level
*/
public class HvdcLine : AuditableEntity
{
    public required Bus Bus1 { get; set; }
    public int Bus1Id { get; set; }

    public required Bus Bus2 { get; set; }
    public int Bus2Id { get; set; }
    public double Length { get; set; }
    public required string ConductorType { get; set; }
    public bool IsSpsPresent { get; set; }
}
