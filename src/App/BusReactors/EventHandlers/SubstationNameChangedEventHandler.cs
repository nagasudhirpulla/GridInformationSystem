using App.BusReactors.Utils;
using App.Common.Interfaces;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.BusReactors.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get bus reactors at the desired substation
        var busReactors = await context.BusReactors
            .Where(s => s.Substation1Id == notification.Substation.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var b in busReactors)
        {
            var newBrName = DeriveBusReactorName.Execute(notification.Substation.Name, b.ElementNumber);
            b.Name = newBrName;
        }
    }
}