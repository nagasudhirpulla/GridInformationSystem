using App.Bays.Utils;
using App.Common.Interfaces;
using Core.Events.Elements;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Bays.EventHandlers;

public class ElementNameChangedEventHandler(ILogger<ElementNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<ElementNameChangedEvent>
{
    public async Task Handle(ElementNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get bays with desired voltage level
        var bays = await context.Bays
            .Include(s => s.Element1)
            .Include(s => s.Element2)
            .Where(s => (s.Element1Id == notification.Element.Id)||(s.Element2Id==notification.Element.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var b in bays)
        {
            var newName = DeriveBayName.Execute(b.Element1.Name, b.Element2.Name, b.BayType);
            b.Name = newName;
        }
    }
}

