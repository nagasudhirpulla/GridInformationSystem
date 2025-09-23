using App.Common.Interfaces;
using App.Common.Security;
using App.Config;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.ApiClients.Queries.GetApiClientByKey;

[Authorize]
public record GetApiClientJwtQuery : IRequest<JwtSecurityToken?>
{
    public required string Key { get; init; }
}

public class GetApiClientByKeyQueryHandler(IApplicationDbContext context, JwtConfig jwtCfg) : IRequestHandler<GetApiClientJwtQuery, JwtSecurityToken?>
{
    public async Task<JwtSecurityToken?> Handle(GetApiClientJwtQuery request, CancellationToken cancellationToken)
    {
        var client = await context.ApiClients.AsNoTracking()
            .Include(s => s.ApiClientRoles)
            .ThenInclude(ar => ar.ApiRole)
            .Where(ac => ac.Key == request.Key)
            .FirstOrDefaultAsync(cancellationToken);
        if (client != null)
        {
            List<Claim> authClaims = [
                                        new(ClaimTypes.Name, client.Name),
                                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                     ];

            foreach (var role in client.ApiClientRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ApiRole.Name));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtCfg.Secret));

            var token = new JwtSecurityToken(
                issuer: jwtCfg.ValidIssuer,
                audience: jwtCfg.ValidAudience,
                expires: DateTime.Now.AddMinutes(jwtCfg.ExpirationMins),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        return null;
    }
}

