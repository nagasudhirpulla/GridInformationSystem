using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.BusReactors.Commands.DeleteBusReactor;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteBusReactorCommand(int Id) : IRequest;

public class DeleteBusReactorCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteBusReactorCommand>
{
    public async Task Handle(DeleteBusReactorCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.BusReactors
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.BusReactors.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}