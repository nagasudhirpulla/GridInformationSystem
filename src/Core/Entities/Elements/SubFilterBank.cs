namespace Core.Entities.Elements;

/*
 * FilterBankId, SubFilterTag combination is unique
 */

public class SubFilterBank : Element
{
    public required FilterBank FilterBank { get; set; }
    public int FilterBankId { get; set; }

    public required string SubFilterTag { get; set; }

    public double Mvar { get; set; }

    public bool IsSwitchable { get; set; }
}