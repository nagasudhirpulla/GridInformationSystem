using App.ApiClients.Queries.CheckApiClientRole;
using App.Common.Interfaces;
using App.Common.Models;
using MediatR;

namespace Infra.Identity;

public class ApiIdentityService(IMediator mediator) : IIdentityService
{
    public Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        throw new NotImplementedException();
    }

    public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string displayName)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        return await Task.FromResult(userId);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        // check if api client is having a role
        var isInRole = await mediator.Send(new CheckApiClientRoleQuery() { Name = userId, Role = role });
        return isInRole;
    }
}