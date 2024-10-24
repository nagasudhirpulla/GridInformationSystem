using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Regions.Commands.DeleteRegion;

public record DeleteRegionCommand(int Id) : IRequest;

public class DeleteRegionCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteRegionCommand>
{
    public async Task Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Regions
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Regions.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}