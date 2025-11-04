using App.Common.Interfaces;
using App.HvdcPoles.Utils;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.HvdcPoles.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get poles at the desired substation
        var poles = await context.HvdcPoles
            .Where(s => s.Substation1Id == notification.Substation.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var p in poles)
        {
            var newBusName = DeriveHvdcPoleName.Execute(notification.Substation.Name, p.ElementNumber);
            p.Name = newBusName;
        }
    }
}
