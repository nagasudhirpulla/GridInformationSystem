using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Datasources.Commands.DeleteDatasource;

public record DeleteDatasourceCommand(int Id) : IRequest;

public class DeleteDatasourceCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteDatasourceCommand>
{
    public async Task Handle(DeleteDatasourceCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Datasources
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Datasources.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
