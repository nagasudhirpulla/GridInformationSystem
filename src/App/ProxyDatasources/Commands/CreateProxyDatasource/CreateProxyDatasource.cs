using App.Common.Interfaces;
using Core.Entities.Data;
using MediatR;

namespace App.ProxyDatasources.Commands.CreateProxyDatasource;

public record CreateProxyDatasourceCommand : IRequest<int>
{
    public required string Name { get; init; }
    public required string BaseUrl { get; init; }
    public string? ApiKey { get; init; }
}

public class CreateProxyDatasourceCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateProxyDatasourceCommand, int>
{
    public async Task<int> Handle(CreateProxyDatasourceCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProxyDatasource() { Name = request.Name, BaseUrl = request.BaseUrl, ApiKey = request.ApiKey };

        context.ProxyDatasources.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
