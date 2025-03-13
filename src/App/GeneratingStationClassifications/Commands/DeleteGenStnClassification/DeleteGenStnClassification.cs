using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.GeneratingStationClassifications.Commands.DeleteGenStnClassification;

public record DeleteGenStnClassificationCommand(int Id) : IRequest;

public class DeleteGenStnClassificationCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGenStnClassificationCommand>
{
    public async Task Handle(DeleteGenStnClassificationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GeneratingStationClassifications
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.GeneratingStationClassifications.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
