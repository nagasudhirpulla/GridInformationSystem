using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.LineReactors.Commands.DeleteLineReactor;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteLineReactorCommand(int Id) : IRequest;

public class DeleteLineReactorCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteLineReactorCommand>
{
    public async Task Handle(DeleteLineReactorCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.LineReactors
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.LineReactors.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}