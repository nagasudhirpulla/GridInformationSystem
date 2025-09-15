using App.Common.Interfaces;
using Core.Entities.Data;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace App.ApiRoles.Utils;

public static class ApiRoleUtils
{
    public static async Task<List<ApiRole>> GetApiRolesFromIdsAsync(string roleIds, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        List<ApiRole> roles = [];
        var idStrings = roleIds.Split(",").Distinct();
        foreach (string roleId in idStrings)
        {
            ApiRole role = await context.ApiRoles.FirstOrDefaultAsync(o => o.Id == Int32.Parse(roleId), cancellationToken: cancellationToken)
                                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "ApiRole Id is not present in database"
                                                                               }]);
            roles.Add(role);
        }
        return roles;
    }

    public static bool BeValidApiRoleIds(string roleIds)
    {
        return roleIds.Split(',').Any(rId =>
        {
            int roleId = -1;
            try
            {
                roleId = Int32.Parse(rId);
            }
            catch (FormatException)
            {

                return false;
            }
            return true;
        });
    }
}
