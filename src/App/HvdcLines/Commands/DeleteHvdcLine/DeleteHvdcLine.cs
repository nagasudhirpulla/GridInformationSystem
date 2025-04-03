using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcLines.Commands.DeleteHvdcLine;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record DeleteHvdcLineCommand(int Id) : IRequest;

public class DeleteHvdcLineCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteHvdcLineCommand>
{
    public async Task Handle(DeleteHvdcLineCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.HvdcLines
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.HvdcLines.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
