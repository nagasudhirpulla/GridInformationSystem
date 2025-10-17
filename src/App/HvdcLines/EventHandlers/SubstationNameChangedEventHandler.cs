using App.Common.Interfaces;
using App.HvdcLines.Utils;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.HvdcLines.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get bus reactors at the desired substation
        var hvdcLines = await context.HvdcLines
            .Where(e => (e.Substation1Id == notification.Substation.Id) || (e.Substation2Id==notification.Substation.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var hl in hvdcLines)
        {
            var newName = await DeriveHvdcLineName.FromEntity(hl.Id, context, cancellationToken);
            hl.Name = newName;
        }
    }
}


