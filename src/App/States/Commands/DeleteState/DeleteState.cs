using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.States.Commands.DeleteState;

public record DeleteStateCommand(int Id) : IRequest;

public class DeleteStateCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteStateCommand>
{
    public async Task Handle(DeleteStateCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.States
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.States.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}