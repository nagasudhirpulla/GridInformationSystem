using App.Buses.Utils;
using App.Common.Interfaces;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Buses.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get buses at the desired substation
        var buses = await context.Buses
            .Where(s => s.Substation1Id == notification.Substation.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var b in buses)
        {
            var newBusName = DeriveBusName.Execute(notification.Substation.Name, b.BusType, b.ElementNumber);
            b.Name = newBusName;
            // TODO emit bus name changed event
        }
    }
}
