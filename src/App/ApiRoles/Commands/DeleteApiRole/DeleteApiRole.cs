using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiRoles.Commands.DeleteApiRole;

public record DeleteApiRoleCommand(int Id) : IRequest;

public class DeleteApiRoleCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteApiRoleCommand>
{
    public async Task Handle(DeleteApiRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ApiRoles
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.ApiRoles.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
