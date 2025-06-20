using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Fuels.Commands.DeleteFuel;

public record DeleteFuelCommand(int Id) : IRequest;

public class DeleteFuelCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteFuelCommand>
{
    public async Task Handle(DeleteFuelCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Fuels
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Fuels.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}