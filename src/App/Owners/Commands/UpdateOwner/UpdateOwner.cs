using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.Owners.Commands.UpdateOwner;

public record UpdateOwnerCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
}

public class UpdateOwnerCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateOwnerCommand>
{
    public async Task Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Owners
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);
    }
}
