namespace Core.Entities;

public class GeneratingStation : AuditableEntity
{
    public required string Name { get; set; }

    public required string OwnerNamesCache { get; set; }

    public required double Installedcapacity { get; set; }
    public required double MVAcapacity { get; set; }

    public int GeneratingStationClassificationId { get; set; }
    public required GeneratingStationClassification GeneratingStationClassification { get; set; }

    public int GeneratingStationTypeId { get; set; }
    public required GeneratingStationType GeneratingStationType { get; set; }

    public int FuelId { get; set; }
    public required Fuel Fuel { get; set; }

    public int LocationId { get; set; }
    public required Location Location { get; set; }

    public required string RegionCache { get; set; }

    public double Latitude {  get; set; }
    public double Longitude {  get; set; }
}