using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.Datasources.Commands.UpdateDatasource;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateDatasourceCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
}

public class UpdateDatasourceCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateDatasourceCommand>
{
    public async Task Handle(UpdateDatasourceCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Datasources
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        var existingDatasourceName = entity.Name;

        // update entity attributes
        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);
    }
}

