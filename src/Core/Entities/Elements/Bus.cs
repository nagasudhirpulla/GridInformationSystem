using Core.Enums;

namespace Core.Entities.Elements;


public class Bus : Element
{
    public BusTypeEnum BusType { get; set; } = null!;
}
