using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.ApiRoles.Commands.UpdateApiRole;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateApiRoleCommand : IRequest
{
    public int Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateApiRoleCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateApiRoleCommand>
{
    public async Task Handle(UpdateApiRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ApiRoles
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // update entity attributes
        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);

    }
}
