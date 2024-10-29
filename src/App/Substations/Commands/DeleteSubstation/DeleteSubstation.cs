using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Substations.Commands.DeleteSubstation;

public record DeleteSubstationCommand(int Id) : IRequest;

public class DeleteSubstationCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteSubstationCommand>
{
    public async Task Handle(DeleteSubstationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Substations
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Substations.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}