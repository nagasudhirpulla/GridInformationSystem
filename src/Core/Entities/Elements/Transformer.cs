using Core.Enums;

namespace Core.Entities.Elements;

public class Transformer : Element
{
    public TransformerTypeEnum TransformerType { get; set; } = null!;

    public double MvaCapacity { get; set; }
    public string? Make { get; set; }
}
