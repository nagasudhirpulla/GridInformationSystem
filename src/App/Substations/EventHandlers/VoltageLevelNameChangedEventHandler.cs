using App.Common.Interfaces;
using App.Substations.Utils;
using Core.Events.Substations;
using Core.Events.VoltageLevels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Substations.EventHandlers;

public class VoltageLevelNameChangedEventHandler(ILogger<VoltageLevelNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<VoltageLevelNameChangedEvent>
{
    public async Task Handle(VoltageLevelNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get substations with desired voltage level
        var substations = await context.Substations
            .Include(s => s.Location)
            .Where(s => s.VoltageLevelId == notification.VoltageLevel.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var sb in substations)
        {
            var newSubstationName = SubstationUtils.DeriveSubstationName(notification.VoltageLevel.Level, sb.Location.Name);
            sb.Name = newSubstationName;
            sb.AddDomainEvent(new SubstationNameChangedEvent(sb));
        }
    }
}
