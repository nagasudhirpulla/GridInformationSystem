using App.Common.Behaviours;
using App.Common.Interfaces;
using Ardalis.GuardClauses;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.ApiClients.Commands.UpdateApiClient;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateApiClientCommand : IRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public required string ApiRoleIds { get; set; }
}

public class UpdateApiClientCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateApiClientCommand>
{
    public async Task Handle(UpdateApiClientCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ApiClients
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // update entity attributes
        entity.Name = request.Name;
        entity.Key = request.Key;

        // update roleIds if required
        var existingApiRoleRecords = await context.ApiClientRoles.Where(s => s.ApiClientId == request.Id).ToListAsync(cancellationToken: cancellationToken);
        var existingApiRoleIds = existingApiRoleRecords.Select(o => o.ApiRoleId).ToList();
        var newApiRoleIds = request.ApiRoleIds.Split(',').Select(int.Parse).ToList();

        var roleIdsToAdd = newApiRoleIds.Where(o => !existingApiRoleIds.Contains(o)).ToList();
        var roleIdsToDelete = existingApiRoleIds.Where(o => !newApiRoleIds.Contains(o)).ToList();

        if (roleIdsToAdd.Count > 0 || roleIdsToDelete.Count > 0)
        {
            foreach (var roleId in roleIdsToAdd)
            {
                context.ApiClientRoles.Add(new ApiClientRole
                {
                    ApiClientId = request.Id,
                    ApiRoleId = roleId
                });
            }

            foreach (ApiClientRole apiClientRole in existingApiRoleRecords.Where(ar => roleIdsToDelete.Contains(ar.ApiRoleId)))
            {

                context.ApiClientRoles.Remove(apiClientRole);
            }

        }

        // save changes to DB
        await context.SaveChangesAsync(cancellationToken);
    }
}

