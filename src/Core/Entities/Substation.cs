namespace Core.Entities;

public class Substation : AuditableEntity
{
    public required string NameCache { get; set; }

    public required string OwnerNamesCache { get; set; }

    public int VoltageLevelId { get; set; }

    public required VoltageLevel VoltageLevel { get; set; }

    public int LocationId { get; set; }
    public required Location Location { get; set; }

    public required bool IsAc { get; set; } = true;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}