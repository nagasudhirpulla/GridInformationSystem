using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Locations.Commands.DeleteLocation;

public record DeleteLocationCommand(int Id) : IRequest;

public class DeleteLocationCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteLocationCommand>
{
    public async Task Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Locations
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Locations.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}