using Core.Entities.Common;
using Core.Entities.Elements;

namespace Core.Events.Elements;

public class ElementNameChangedEvent(Element el) : BaseEvent
{
    public Element Element { get; } = el;
}