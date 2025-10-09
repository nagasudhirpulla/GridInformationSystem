using Core.Entities;
using Core.Entities.Common;

namespace Core.Events.Substations;

public class SubstationNameChangedEvent(Substation substation) : BaseEvent
{
    public Substation Substation { get; } = substation;
}