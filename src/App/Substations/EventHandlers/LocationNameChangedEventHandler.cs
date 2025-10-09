using App.Common.Interfaces;
using App.Substations.Utils;
using Core.Events.Locations;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Substations.EventHandlers;

public class LocationNameChangedEventHandler(ILogger<LocationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<LocationNameChangedEvent>
{
    public async Task Handle(LocationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get substations at the desired location
        var substations = await context.Substations
            .Include(s => s.VoltageLevel)
            .Where(s => s.LocationId == notification.Location.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var sb in substations)
        {
            var newSubstationName = SubstationUtils.DeriveSubstationName(sb.VoltageLevel.Level, notification.Location.Name);
            sb.Name = newSubstationName;
            sb.AddDomainEvent(new SubstationNameChangedEvent(sb));
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}
