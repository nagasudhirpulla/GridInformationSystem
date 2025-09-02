using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ProxyDatasources.Commands.DeleteProxyDatasource;

public record DeleteProxyDatasourceCommand(int Id) : IRequest;

public class DeleteProxyDatasourceCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteProxyDatasourceCommand>
{
    public async Task Handle(DeleteProxyDatasourceCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ProxyDatasources
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.ProxyDatasources.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
