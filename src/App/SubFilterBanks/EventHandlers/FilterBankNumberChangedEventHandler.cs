using App.Common.Interfaces;
using App.SubFilterBanks.Utils;
using Core.Events.FilterBanks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.SubFilterBanks.EventHandlers;

public class FilterBankNumberChangedEventHandler(ILogger<FilterBankNumberChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<FilterBankNumberChangedEvent>
{
    public async Task Handle(FilterBankNumberChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get subFilterBanks with desired voltage level
        var subFilterBanks = await context.SubFilterBanks
            .Include(s => s.Substation1)
            .Where(s => s.FilterBankId == notification.FilterBank.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var sb in subFilterBanks)
        {
            var newSubstationName = DeriveSubFilterBankName.Execute(sb.Substation1.Name, notification.FilterBank.ElementNumber, sb.ElementNumber);
            sb.Name = newSubstationName;
        }
    }
}
