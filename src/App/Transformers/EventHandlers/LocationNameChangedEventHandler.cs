using App.Common.Interfaces;
using App.Transformers.Utils;
using Core.Events.Locations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Transformers.EventHandlers;

public class LocationNameChangedEventHandler(ILogger<LocationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<LocationNameChangedEvent>
{
    public async Task Handle(LocationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get transformers at the desired location
        var transformers = await context.Transformers
            .Include(s => s.Substation1.VoltageLevel)
            .Include(s => s.Substation2.VoltageLevel)
            .Where(s => s.Substation1.LocationId == notification.Location.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var t in transformers)
        {
            var newName = DeriveTransformerName.Execute(notification.Location.Name, t.Substation1.VoltageLevel.Level, t.Substation2.VoltageLevel.Level, t.ElementNumber, t.TransformerType);
            t.Name = newName;
        }
    }
}
