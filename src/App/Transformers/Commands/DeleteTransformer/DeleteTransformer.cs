using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Transformers.Commands.DeleteTransformer;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteTransformerCommand(int Id) : IRequest;

public class DeleteTransformerCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTransformerCommand>
{
    public async Task Handle(DeleteTransformerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Transformers
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Transformers.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
