namespace Core.Entities.Elements;

/*
Generating voltage will be less than Generating Transformer Voltage
Generating Unit should be present in Generating station instead of Substation
*/
public class GeneratingUnit : Element
{
    public double InstalledCapacity { get; set; }

    public double GeneratingVoltage { get; set; }
}