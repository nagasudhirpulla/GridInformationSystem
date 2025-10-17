using App.Common.Interfaces;
using App.FilterBanks.Utils;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.FilterBanks.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get filter banks at the desired substation
        var filterBanks = await context.FilterBanks
            .Where(s => s.Substation1Id == notification.Substation.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var e in filterBanks)
        {
            var newName = DeriveFilterBankName.Execute(notification.Substation.Name, e.ElementNumber);
            e.Name = newName;
        }
    }
}
