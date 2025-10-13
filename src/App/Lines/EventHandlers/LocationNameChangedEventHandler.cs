using App.Common.Interfaces;
using App.Lines.Utils;
using Core.Events.Locations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Lines.EventHandlers;

public class LocationNameChangedEventHandler(ILogger<LocationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<LocationNameChangedEvent>
{
    public async Task Handle(LocationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get lines at the desired substation
        var lines = await context.Lines
            .Include(l => l.Substation1.VoltageLevel)
            .Include(l => l.Substation1.Location)
            .Include(l => l.Substation2.Location)
            .Where(s => (s.Substation1.LocationId == notification.Location.Id) || (s.Substation2.LocationId == notification.Location.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var l in lines)
        {
            string location1Name = l.Substation1.LocationId == notification.Location.Id ? notification.Location.Name : l.Substation1.Location.Name;
            string location2Name = l.Substation2.LocationId == notification.Location.Id ? notification.Location.Name : l.Substation2.Location.Name;
            var newName = DeriveLineName.Execute(l.Substation1.VoltageLevel.Level, location1Name, location2Name, l.ElementNumber);
            l.Name = newName;
        }
    }
}
