using Core.Entities.Common;
using Core.Entities.Elements;

namespace Core.Events.Buses;

public class BusNameChangedEvent(Bus bus) : BaseEvent
{
    public Bus Bus { get; } = bus;
}
