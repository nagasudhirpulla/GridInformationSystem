namespace Core.Entities;

/*
Poletype can be Inverter, Rectifier
Combination of Substation and PoleNumber should be unique
Substation should be HVDC substation only
*/
public class HvdcPole : AuditableEntity
{
    public required string PoleType { get; set; }
}
