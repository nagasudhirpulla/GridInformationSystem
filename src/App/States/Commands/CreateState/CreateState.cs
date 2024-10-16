using App.Common.Interfaces;
using Core.Entities;
using MediatR;

namespace App.States.Commands.CreateState;

public record CreateStateCommand : IRequest<int>
{
    public required string Name { get; init; }
    public int RegionId { get; set; }
}

public class CreateStateCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateStateCommand, int>
{
    public async Task<int> Handle(CreateStateCommand request, CancellationToken cancellationToken)
    {
        var entity = new State() { Name = request.Name, RegionId = request.RegionId };

        context.States.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
