using Core.Entities.Common;
using Core.Entities.Elements;

namespace Core.Events.FilterBanks;

public class FilterBankNumberChangedEvent(FilterBank fb) : BaseEvent
{
    public FilterBank FilterBank { get; } = fb;
}