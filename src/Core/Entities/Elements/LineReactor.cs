namespace Core.Entities.Elements;

/*
 * line, substation, line reactor number combination is unique
 */
public class LineReactor : Element
{
    public Line Line { get; set; } = null!;
    public int LineId { get; set; }

    public double MvarCapacity { get; set; }

    public bool IsConvertible { get; set; }

    public bool IsSwitchable { get; set; }
}
