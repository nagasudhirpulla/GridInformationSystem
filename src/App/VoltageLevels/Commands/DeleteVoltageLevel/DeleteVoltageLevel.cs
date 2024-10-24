using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.VoltageLevels.Commands.DeleteVoltageLevel;

public record DeleteVoltageLevelCommand(int Id) : IRequest;

public class DeleteVoltageLevelCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteVoltageLevelCommand>
{
    public async Task Handle(DeleteVoltageLevelCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.VoltageLevels
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.VoltageLevels.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}