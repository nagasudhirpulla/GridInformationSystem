using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Owners.Commands.DeleteOwner;

public record DeleteOwnerCommand(int Id) : IRequest;

public class DeleteOwnerCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteOwnerCommand>
{
    public async Task Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Owners
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Owners.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}