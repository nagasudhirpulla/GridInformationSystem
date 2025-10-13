using App.Common.Interfaces;
using App.Lines.Utils;
using Core.Events.VoltageLevels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Lines.EventHandlers;

public class VoltageLevelNameChangedEventHandler(ILogger<VoltageLevelNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<VoltageLevelNameChangedEvent>
{
    public async Task Handle(VoltageLevelNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get the lines with desired voltage level
        var lines = await context.Lines
            .Include(l => l.Substation1)
            .ThenInclude(s => s.Location)
            .Include(l => l.Substation2)
            .ThenInclude(s => s.Location)
            .Where(l => l.Substation1.VoltageLevelId == notification.VoltageLevel.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var l in lines)
        {
            var newLineName = DeriveLineName.Execute(notification.VoltageLevel.Level, l.Substation1.Location.Name, l.Substation2?.Location?.Name ?? "", l.ElementNumber);
            l.Name = newLineName;
        }
    }
}
