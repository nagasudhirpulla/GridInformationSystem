namespace Core.Entities;

/*
 * bus, substation, bus reactor number combination is unique
 */
public class BusReactor : AuditableEntity
{
    public required Bus Bus { get; set; }
    public int BusId { get; set; }
    public double MvarCapacity { get; set; }
}
