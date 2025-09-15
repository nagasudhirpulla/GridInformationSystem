using App.Common.Interfaces;
using Core.Entities.Data;
using MediatR;

namespace App.ApiRoles.Commands.CreateApiRole;

public record CreateApiRoleCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateApiRoleCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateApiRoleCommand, int>
{
    public async Task<int> Handle(CreateApiRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = new ApiRole() { Name = request.Name };

        context.ApiRoles.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
