using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.Fuels.Commands.UpdateFuel;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateFuelCommand : IRequest
{
    public int Id { get; init; }

    public required string FuelName { get; init; }
}

public class UpdateFuelCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateFuelCommand>
{
    public async Task Handle(UpdateFuelCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Fuels
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        var existingFuelName = entity.Name;

        // update entity attributes
        entity.Name = request.FuelName;

        await context.SaveChangesAsync(cancellationToken);

    }
}
