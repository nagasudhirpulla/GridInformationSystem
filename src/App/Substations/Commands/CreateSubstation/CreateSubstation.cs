using App.Common.Interfaces;
using MediatR;

namespace App.Substations.Commands.CreateSubstation;

public record CreateSubstationCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateSubstationCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateSubstationCommand, int>
{
    public async Task<int> Handle(CreateSubstationCommand request, CancellationToken cancellationToken)
    {
        // TODO complete this
        //var entity = new Substation() { Name = request.Name };

        //context.Substations.Add(entity);

        //await context.SaveChangesAsync(cancellationToken);

        //return entity.Id;

        return -1;
    }
}