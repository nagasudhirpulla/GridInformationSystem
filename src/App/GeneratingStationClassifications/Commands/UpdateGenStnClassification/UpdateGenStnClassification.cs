using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.GeneratingStationClassifications.Commands.UpdateGenStnClassification;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateGenStnClassificationCommand : IRequest
{
    public int Id { get; init; }

    public required string Classification { get; init; }
}

public class UpdateGenStnClassificationCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateGenStnClassificationCommand>
{
    public async Task Handle(UpdateGenStnClassificationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GeneratingStationClassifications
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // update entity attributes
        entity.Classification = request.Classification;

        await context.SaveChangesAsync(cancellationToken);

    }
}
