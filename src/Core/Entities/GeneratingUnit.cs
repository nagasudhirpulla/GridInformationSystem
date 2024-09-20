namespace Core.Entities;

/*
Generating voltage will be less than Generating Transformer Voltage
*/
public class GeneratingUnit : AuditableEntity
{
    public double InstalledCapacity { get; set; }
    public double GeneratingVoltage { get; set; }
}