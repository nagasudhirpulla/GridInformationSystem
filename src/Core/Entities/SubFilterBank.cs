namespace Core.Entities;

/*
 * FilterBankId, SubFilterTag combination is unique
 */

public class SubFilterBank : AuditableEntity
{
    public int ElementId { get; set; }
    public required Element Element { get; set; }

    public int FilterBankId { get; set; }
    public required FilterBank FilterBank { get; set; }

    public required string SubFilterTag { get; set; }

    public int VoltageLevelId { get; set; }
    public required VoltageLevel VoltageLevel { get; set; }

    public double Mvar { get; set; }

    public bool IsSwitchable { get; set; }
}