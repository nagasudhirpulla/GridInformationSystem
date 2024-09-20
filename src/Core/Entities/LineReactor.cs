namespace Core.Entities;

/*
 * line, substation, line reactor number combination is unique
 */
public class LineReactor : AuditableEntity
{
    public required Line Line { get; set; }
    public int LineId { get; set; }
    public double MvarCapacity { get; set; }
    public bool IsConvertible { get; set; }
    public bool IsSwitchable { get; set; }
}
