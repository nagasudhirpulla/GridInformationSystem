using App.Common.Interfaces;
using Core.Entities;
using MediatR;

namespace App.GeneratingStationTypes.Commands.CreateGenStationType;

public record CreateGenStationTypeCommand : IRequest<int>
{
    public required string StationType { get; init; }
}

public class CreateGenStationTypeCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateGenStationTypeCommand, int>
{
    public async Task<int> Handle(CreateGenStationTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new GeneratingStationType() { StationType = request.StationType };

        context.GeneratingStationTypes.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
