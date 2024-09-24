using Core.Enums;

namespace Core.Entities.Elements;

/*
Poletype can be Inverter, Rectifier
Combination of Substation and PoleNumber should be unique
Substation should be HVDC substation only
*/
public class HvdcPole : Element
{
    public required HvdcPoleTypeEnum PoleType { get; set; }
}
