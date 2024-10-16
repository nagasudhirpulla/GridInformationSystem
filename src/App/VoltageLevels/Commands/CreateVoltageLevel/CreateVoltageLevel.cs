using App.Common.Interfaces;
using Core.Entities;
using MediatR;

namespace App.VoltageLevels.Commands.CreateVoltageLevel;

public record CreateVoltageLevelCommand : IRequest<int>
{
    public required string Level { get; init; }
}

public class CreateVoltageLevelCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateVoltageLevelCommand, int>
{
    public async Task<int> Handle(CreateVoltageLevelCommand request, CancellationToken cancellationToken)
    {
        var entity = new VoltageLevel() { Level = request.Level };

        context.VoltageLevels.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

