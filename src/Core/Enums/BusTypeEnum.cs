using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class BusTypeEnum : SmartEnum<BusTypeEnum>
{
    public static readonly BusTypeEnum Main = new(nameof(Main), 1);
    public static readonly BusTypeEnum Transfer = new(nameof(Transfer), 2);
    public static readonly BusTypeEnum Auxiliary = new(nameof(Auxiliary), 3);

    private BusTypeEnum(string name, int value) : base(name, value)
    {
    }
}
