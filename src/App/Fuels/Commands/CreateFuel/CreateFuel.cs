using App.Common.Interfaces;
using Core.Entities;
using MediatR;

namespace App.Fuels.Commands.CreateFuel;

public record CreateFuelCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateFuelCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateFuelCommand, int>
{
    public async Task<int> Handle(CreateFuelCommand request, CancellationToken cancellationToken)
    {
        var entity = new Fuel() { Name = request.Name };

        context.Fuels.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
