using Core.Entities.Common;
using Core.Entities.Elements;

namespace Core.Events.Lines;

public class LineNameChangedEvent(Line line) : BaseEvent
{
    public Line Line { get; } = line;
}