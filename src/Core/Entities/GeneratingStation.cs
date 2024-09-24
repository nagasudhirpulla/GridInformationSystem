namespace Core.Entities;

public class GeneratingStation : Substation
{
    public required string Name { get; set; }

    public required double Installedcapacity { get; set; }

    public required double MVAcapacity { get; set; }

    public required GeneratingStationClassification GeneratingStationClassification { get; set; }
    public int GeneratingStationClassificationId { get; set; }

    public required GeneratingStationType GeneratingStationType { get; set; }
    public int GeneratingStationTypeId { get; set; }

    public required Fuel Fuel { get; set; }
    public int FuelId { get; set; }

}