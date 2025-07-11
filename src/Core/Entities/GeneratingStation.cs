namespace Core.Entities;

public class GeneratingStation : Substation
{
    public required string Name { get; set; }

    public required double InstalledCapacity { get; set; }

    public required double MvaCapacity { get; set; }

    public GeneratingStationClassification GeneratingStationClassification { get; set; } = null!;
    public int GeneratingStationClassificationId { get; set; }

    public GeneratingStationType GeneratingStationType { get; set; } = null!;
    public int GeneratingStationTypeId { get; set; }

    public Fuel Fuel { get; set; } = null!;
    public int FuelId { get; set; }

}