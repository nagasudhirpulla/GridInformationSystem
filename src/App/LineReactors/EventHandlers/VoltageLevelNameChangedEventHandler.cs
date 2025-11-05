using App.Common.Interfaces;
using App.LineReactors.Utils;
using Core.Events.Substations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.LineReactors.EventHandlers;

public class SubstationNameChangedEventHandler(ILogger<SubstationNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<SubstationNameChangedEvent>
{
    public async Task Handle(SubstationNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get the lineReactors with desired voltage level
        var lineReactors = await context.LineReactors
            .Include(l => l.Line)
            .Where(l => l.Substation1Id == notification.Substation.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var l in lineReactors)
        {
            var newLineReactorName = DeriveLineReactorName.Execute(l.Line.Name, l.Substation1.Name, l.ElementNumber);
            l.Name = newLineReactorName;
        }
    }
}
