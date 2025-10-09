using Core.Entities;
using Core.Entities.Common;

namespace Core.Events.Locations;

public class LocationNameChangedEvent(Location location) : BaseEvent
{
    public Location Location { get; } = location;
}
