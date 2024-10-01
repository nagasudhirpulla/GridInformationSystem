namespace Core.Entities.Elements;

/*
 * FilterBankId, SubFilterTag combination is unique
 */

public class SubFilterBank : Element
{
    public FilterBank FilterBank { get; set; } = null!;
    public int FilterBankId { get; set; }

    public required string SubFilterTag { get; set; }

    public double Mvar { get; set; }

    public bool IsSwitchable { get; set; }
}