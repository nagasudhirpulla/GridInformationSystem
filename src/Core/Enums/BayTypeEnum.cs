using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class BayTypeEnum : SmartEnum<BayTypeEnum>
{
    public static readonly BayTypeEnum TieBay = new(nameof(TieBay), 1);
    public static readonly BayTypeEnum MainBay = new(nameof(MainBay), 2);
    public static readonly BayTypeEnum BusCouplerBay = new(nameof(BusCouplerBay), 3);
    public static readonly BayTypeEnum BusSectionalizerBay = new(nameof(BusSectionalizerBay), 4);
    public static readonly BayTypeEnum TBCBay = new(nameof(TBCBay), 5);

    private BayTypeEnum(string name, int value) : base(name, value)
    {
    }
}