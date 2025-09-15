using App.ApiRoles.Utils;
using App.Common.Behaviours;
using App.Common.Interfaces;
using Core.Entities.Data;
using MediatR;

namespace App.ApiClients.Commands.CreateApiClient;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateApiClientCommand : IRequest<int>
{
    public required string ApiRoleIds { get; init; }
    public required string Name { get; set; }
    public required string Key { get; set; }
}

public class CreateApiClientCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateApiClientCommand, int>
{
    public async Task<int> Handle(CreateApiClientCommand request, CancellationToken cancellationToken)
    {
        List<ApiRole> roles = await ApiRoleUtils.GetApiRolesFromIdsAsync(request.ApiRoleIds, context, cancellationToken);

        var entity = new ApiClient()
        {
            Name = request.Name,
            Key = request.Key,
        };

        context.ApiClients.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ApiClientRole objects
        foreach (ApiRole role in roles)
        {
            var apiClientRole = new ApiClientRole
            {
                ApiRoleId = role.Id,
                ApiClientId = entity.Id
            };
            context.ApiClientRoles.Add(apiClientRole);
        }
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
