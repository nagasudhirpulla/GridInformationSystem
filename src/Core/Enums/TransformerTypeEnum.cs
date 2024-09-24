using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class TransformerTypeEnum : SmartEnum<TransformerTypeEnum>
{
    public static readonly TransformerTypeEnum ICT = new(nameof(ICT), 1);
    public static readonly TransformerTypeEnum GT = new(nameof(GT), 2);

    private TransformerTypeEnum(string name, int value) : base(name, value)
    {
    }
}