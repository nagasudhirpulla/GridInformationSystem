using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class HvdcPoleTypeEnum : SmartEnum<HvdcPoleTypeEnum>
{
    public static readonly HvdcPoleTypeEnum Rectifier = new(nameof(Rectifier), 1);
    public static readonly HvdcPoleTypeEnum Inverter = new(nameof(Inverter), 2);

    private HvdcPoleTypeEnum(string name, int value) : base(name, value)
    {
    }
}