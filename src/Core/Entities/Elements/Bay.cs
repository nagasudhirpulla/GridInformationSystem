using Core.Enums;

namespace Core.Entities.Elements;

/*
Element1 and Element2 should be present in the same substation
For a Tie Bay Element1 and Element2 would be Non-Bus
For a Main Bay exactly one of the element1 or element2 would be bus
For a Bus Coupler, Bus Sectionalizer, TBC Bay elment1 and element2 both are bus type
TODO bay type can be Tie Bay / Main Bay / Bus Coupler Bay / Bus Sectionalizer Bay / TBC Bay
TODOS
whether to fix element1 as bus for main bay
spare bay column may be added
*/
public class Bay : Element
{
    public Element Element1 { get; set; } = null!;
    public int Element1Id { get; set; }

    public Element Element2 { get; set; } = null!;
    public int Element2Id { get; set; }

    public BayTypeEnum BayType { get; set; } = null!;

    public bool IsFuture { get; set; }
}
