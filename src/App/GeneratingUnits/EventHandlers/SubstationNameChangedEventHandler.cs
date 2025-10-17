using App.Common.Interfaces;
using App.GeneratingUnits.Utils;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.GeneratingUnits.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get bus reactors at the desired substation
        var genUnits = await context.GeneratingUnits
            .Where(s => s.Substation1Id == notification.Substation.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var g in genUnits)
        {
            var newName = DeriveGenUnitName.Execute(notification.Substation.Name, g.ElementNumber);
            g.Name = newName;
        }
    }
}
