using Core.Enums;

namespace Core.Entities.Elements;


public class Bus : Element
{
    public required BusTypeEnum BusType { get; set; }
}
