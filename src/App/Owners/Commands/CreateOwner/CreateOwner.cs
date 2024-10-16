using App.Common.Interfaces;
using Core.Entities;
using MediatR;

namespace App.Owners.Commands.CreateOwner;

public record CreateOwnerCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateOwnerCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateOwnerCommand, int>
{
    public async Task<int> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Owner() { Name = request.Name };

        context.Owners.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
