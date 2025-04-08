namespace Core.Entities.Elements;

/*
 * FilterBankId, SubFilterTag combination is unique
 * SubFilterTag would be ElementNumber field in Element class definition
 */

public class SubFilterBank : Element
{
    public FilterBank FilterBank { get; set; } = null!;
    public int FilterBankId { get; set; }


    public double Mvar { get; set; }

    public bool IsSwitchable { get; set; }
}