using App.Common.Interfaces;
using App.SubFilterBanks.Utils;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.SubFilterBanks.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get subFilterBanks at the desired substation
        var subFilterBanks = await context.SubFilterBanks
            .Include(e => e.FilterBank)
            .Where(s => s.Substation1Id == notification.Substation.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var l in subFilterBanks)
        {
            var newName = DeriveSubFilterBankName.Execute(notification.Substation.Name, l.FilterBank.ElementNumber, l.ElementNumber);
            l.Name = newName;
        }
    }
}
