namespace Core.Entities.Elements;

/*
 * bus, substation, bus reactor number combination is unique
 */
public class BusReactor : Element
{
    public Bus Bus { get; set; } = null!;
    public int BusId { get; set; }

    public double MvarCapacity { get; set; }
}
