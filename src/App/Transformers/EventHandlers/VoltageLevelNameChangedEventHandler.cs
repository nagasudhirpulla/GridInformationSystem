using App.Common.Interfaces;
using App.Transformers.Utils;
using Core.Events.VoltageLevels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace App.Transformers.EventHandlers;

public class VoltageLevelNameChangedEventHandler(ILogger<VoltageLevelNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<VoltageLevelNameChangedEvent>
{
    public async Task Handle(VoltageLevelNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get transformers with desired voltage level
        var transformers = await context.Transformers
            .Include(s => s.Substation1.VoltageLevel)
            .Include(s => s.Substation2.VoltageLevel)
            .Include(s => s.Substation1.Location)
            .Where(s => new int[] { s.Substation1.VoltageLevelId, s.Substation2.VoltageLevelId }.Contains(notification.VoltageLevel.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var t in transformers)
        {
            string level1 = (t.Substation1.VoltageLevelId == notification.VoltageLevel.Id) ? notification.VoltageLevel.Level : t.Substation1.VoltageLevel.Level;
            string level2 = (t.Substation2.VoltageLevelId == notification.VoltageLevel.Id) ? notification.VoltageLevel.Level : t.Substation2.VoltageLevel.Level;
            IOrderedEnumerable<string> hvLvLevels = new[] { level1, level2 }.OrderByDescending(i => int.Parse(Regex.Match(i, @"\d+").Value));
            var newName = DeriveTransformerName.Execute(t.Substation1.Location.Name, hvLvLevels.ElementAt(0), hvLvLevels.ElementAt(1), t.ElementNumber, t.TransformerType);
            t.Name = newName;
        }
    }
}
