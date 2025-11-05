using App.Common.Interfaces;
using App.LineReactors.Utils;
using Core.Events.Lines;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.LineReactors.EventHandlers;

public class LineNameChangedEventHandler(ILogger<LineNameChangedEventHandler> logger, IApplicationDbContext context) : INotificationHandler<LineNameChangedEvent>
{
    public async Task Handle(LineNameChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("App Domain Event: {DomainEvent}", notification.GetType().Name);

        // get the line reactor connected to the line
        var lineReactors = await context.LineReactors
            .Include(l => l.Substation1)
            .Where(l => l.LineId == notification.Line.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var l in lineReactors)
        {
            var newLineReactorName = DeriveLineReactorName.Execute(notification.Line.Name, l.Substation1.Name, l.ElementNumber);
            l.Name = newLineReactorName;
        }
    }
}
