using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Measurements.Commands.DeleteMeasurement;

public record DeleteMeasurementCommand(int Id) : IRequest;

public class DeleteMeasurementCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteMeasurementCommand>
{
    public async Task Handle(DeleteMeasurementCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Measurements
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Measurements.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
