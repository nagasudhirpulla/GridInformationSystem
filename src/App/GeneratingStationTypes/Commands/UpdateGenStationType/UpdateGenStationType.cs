using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.GeneratingStationTypes.Commands.UpdateGenStationType;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateGenStationTypeCommand : IRequest
{
    public int Id { get; init; }

    public required string StationType { get; init; }
}

public class UpdateGenStationTypeCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateGenStationTypeCommand>
{
    public async Task Handle(UpdateGenStationTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GeneratingStationTypes
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        var existingGenStationTypeName = entity.StationType;

        // update entity attributes
        entity.StationType = request.StationType;

        await context.SaveChangesAsync(cancellationToken);
    }
}
