using App.Common.Interfaces;
using Core.Entities.Elements;
using Core.Events.VoltageLevels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Elements.EventHandlers;

public class VoltageLevelNameChangedEventHandler(ILogger<VoltageLevelNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<VoltageLevelNameChangedEvent>
{
    public async Task Handle(VoltageLevelNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get elements with desired voltage level
        var elements = await context.Elements
            .Include(e => e.Substation1)
            .Include(e => e.Substation2)
            .Where(e => (e.Substation1.VoltageLevelId == notification.VoltageLevel.Id) || (e.Substation2.VoltageLevelId == notification.VoltageLevel.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var e in elements)
        {
            string newVoltCache = notification.VoltageLevel.Level;
            if (e is Transformer t)
            {
                newVoltCache = await Transformers.Utils.DeriveTransformerVoltageLevel.FromEntity(t.Id, context, cancellationToken: cancellationToken);
            }
            newVoltCache = notification.VoltageLevel.Level;
            e.VoltageLevelCache = newVoltCache;
        }
    }
}

