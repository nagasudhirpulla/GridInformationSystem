using App.Common.Interfaces;
using App.HvdcLines.Utils;
using Core.Events.VoltageLevels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.HvdcLines.EventHandlers;

public class VoltageLevelNameChangedEventHandler(ILogger<VoltageLevelNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<VoltageLevelNameChangedEvent>
{
    public async Task Handle(VoltageLevelNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get hvdcLines with desired voltage level
        var hvdcLines = await context.HvdcLines
            .Include(e=>e.Substation1)
            .Where(s => s.Substation1.VoltageLevelId == notification.VoltageLevel.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var e in hvdcLines)
        {
            var newName = await DeriveHvdcLineName.FromEntity(e.Id, context, cancellationToken);
            e.Name = newName;
        }
    }
}


