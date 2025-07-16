using App.Common.Interfaces;
using Core.Entities.Data;
using MediatR;

namespace App.Datasources.Commands.CreateDatasource;

public record CreateDatasourceCommand : IRequest<int>
{
    public required string Name { get; init; }
}

public class CreateDatasourceCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateDatasourceCommand, int>
{
    public async Task<int> Handle(CreateDatasourceCommand request, CancellationToken cancellationToken)
    {
        var entity = new Datasource() { Name = request.Name };

        context.Datasources.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
