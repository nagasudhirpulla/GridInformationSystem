using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationTypes.Commands.DeleteGenStationType;

public record DeleteGenStationTypeCommand(int Id) : IRequest;

public class DeleteGenStationTypeCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGenStationTypeCommand>
{
    public async Task Handle(DeleteGenStationTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GeneratingStationTypes
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.GeneratingStationTypes.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}