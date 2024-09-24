namespace Core.Entities.Elements;

/*
 * bus, substation, bus reactor number combination is unique
 */
public class BusReactor : Element
{
    public required Bus Bus { get; set; }
    public int BusId { get; set; }

    public double MvarCapacity { get; set; }
}
