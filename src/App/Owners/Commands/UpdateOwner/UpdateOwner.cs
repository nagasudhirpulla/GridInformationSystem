using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Ardalis.GuardClauses;
using MediatR;

namespace App.Owners.Commands.UpdateOwner;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
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

        string exisitingOwnerName = entity.Name;

        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);

        // update owner name in cache columns of all tables
        await UpdateCacheOnOwnerRename.ExecuteAsync(exisitingOwnerName, request.Name, context, cancellationToken);
    }
}
