using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcPoles.Commands.DeleteHvdcPole;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteHvdcPoleCommand(int Id) : IRequest;

public class DeleteHvdcPoleCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteHvdcPoleCommand>
{
    public async Task Handle(DeleteHvdcPoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.HvdcPoles
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.HvdcPoles.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
