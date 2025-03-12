using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using Core.Entities.Elements;
using MediatR;

namespace App.VoltageLevels.Commands.UpdateVoltageLevel;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateVoltageLevelCommand : IRequest
{
    public int Id { get; init; }

    public required string Level { get; init; }
}

public class UpdateVoltageLevelCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateVoltageLevelCommand>
{
    public async Task Handle(UpdateVoltageLevelCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.VoltageLevels
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        string existingName = entity.Level;

        // update entity attributes
        entity.Level = request.Level;

        await context.SaveChangesAsync(cancellationToken);

        // update voltage level cache column in all tables
        await context.ReplaceSubstringInColumn(existingName, request.Level, nameof(Element.VoltageLevelCache), cancellationToken);
    }
}
