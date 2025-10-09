using Core.Entities;
using Core.Entities.Common;

namespace Core.Events.Locations;

public class LocationStateChangedEvent(Location location) : BaseEvent
{
    public Location Location { get; } = location;
}