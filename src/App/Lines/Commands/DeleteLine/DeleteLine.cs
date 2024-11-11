using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Lines.Commands.DeleteLine;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteLineCommand(int Id) : IRequest;

public class DeleteLineCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteLineCommand>
{
    public async Task Handle(DeleteLineCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Lines
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Lines.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}