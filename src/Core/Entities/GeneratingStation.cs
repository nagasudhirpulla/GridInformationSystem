namespace Core.Entities;

public class GeneratingStation : Substation
{
    public required string Name { get; set; }

    public required double Installedcapacity { get; set; }

    public required double MVAcapacity { get; set; }

    public GeneratingStationClassification GeneratingStationClassification { get; set; } = null!;
    public int GeneratingStationClassificationId { get; set; }

    public GeneratingStationType GeneratingStationType { get; set; } = null!;
    public int GeneratingStationTypeId { get; set; }

    public Fuel Fuel { get; set; } = null!;
    public int FuelId { get; set; }

}