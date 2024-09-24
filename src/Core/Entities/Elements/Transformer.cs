using Core.Enums;

namespace Core.Entities.Elements;

public class Transformer : Element
{
    public required TransformerTypeEnum TransformerType { get; set; }

    public double MvaCapacity { get; set; }
}
