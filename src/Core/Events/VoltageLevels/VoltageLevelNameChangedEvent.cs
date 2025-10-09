using Core.Entities;
using Core.Entities.Common;

namespace Core.Events.VoltageLevels;

public class VoltageLevelNameChangedEvent(VoltageLevel location) : BaseEvent
{
    public VoltageLevel VoltageLevel { get; } = location;
}
