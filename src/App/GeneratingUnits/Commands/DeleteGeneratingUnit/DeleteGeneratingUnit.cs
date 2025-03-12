using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingUnits.Commands.DeleteGeneratingUnit;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteGeneratingUnitCommand(int Id) : IRequest;

public class DeleteGeneratingUnitCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGeneratingUnitCommand>
{
    public async Task Handle(DeleteGeneratingUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GeneratingUnits
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.GeneratingUnits.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
