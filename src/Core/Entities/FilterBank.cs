namespace Core.Entities;


public class FilterBank : AuditableEntity
{
    public int ElementId { get; set; }
    public required Element Element { get; set; }

    public double Mvar { get; set; }

    public bool IsSwitchable { get; set; }
}
