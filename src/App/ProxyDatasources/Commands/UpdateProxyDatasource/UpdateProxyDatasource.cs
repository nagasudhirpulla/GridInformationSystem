using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace App.ProxyDatasources.Commands.UpdateProxyDatasource;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateProxyDatasourceCommand : IRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
    public required string BaseUrl { get; init; }
    public string? ApiKey { get; init; }
}

public class UpdateProxyDatasourceCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateProxyDatasourceCommand>
{
    public async Task Handle(UpdateProxyDatasourceCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ProxyDatasources
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        var existingProxyDatasourceName = entity.Name;

        // update entity attributes
        entity.Name = request.Name;
        entity.BaseUrl = request.BaseUrl;
        entity.ApiKey = request.ApiKey;

        await context.SaveChangesAsync(cancellationToken);

    }
}

