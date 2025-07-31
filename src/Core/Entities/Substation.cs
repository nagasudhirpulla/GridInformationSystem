namespace Core.Entities;

public class Substation : GridEntity
{
    public required string NameCache { get; set; }

    public required string OwnerNamesCache { get; set; }

    public required string RegionCache { get; set; }

    public List<SubstationOwner> SubstationOwners { get; } = [];

    public VoltageLevel VoltageLevel { get; set; } = null!;
    public int VoltageLevelId { get; set; }

    public Location Location { get; set; } = null!;
    public int LocationId { get; set; }

    public bool IsAc { get; set; } = true;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}