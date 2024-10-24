using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.VoltageLevels.Commands.UpdateVoltageLevel;

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

        entity.Level = request.Level;

        await context.SaveChangesAsync(cancellationToken);
    }
}
