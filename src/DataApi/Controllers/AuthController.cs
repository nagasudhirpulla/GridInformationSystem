using App.MeasurementData.Queries.GetMeasurementData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IMediator mediator, ILogger<MeasDataController> logger) : ControllerBase
    {
        private readonly ILogger<MeasDataController> _logger = logger;

        [HttpPost("Login")]
        public async Task<List<(int timestamp, float value)>> Get([FromBody] GetMeasurementDataQuery cmd)
        {
            //var user = await userManager.FindByNameAsync(model.Username);
            //if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            //{
            //    var userRoles = await userManager.GetRolesAsync(user);

            //    var authClaims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, user.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    };

            //    foreach (var userRole in userRoles)
            //    {
            //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            //    }

            //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            //    var token = new JwtSecurityToken(
            //        issuer: _configuration["JWT:ValidIssuer"],
            //        audience: _configuration["JWT:ValidAudience"],
            //        expires: DateTime.Now.AddHours(3),
            //        claims: authClaims,
            //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //        );

            //    return Ok(new
            //    {
            //        token = new JwtSecurityTokenHandler().WriteToken(token),
            //        expiration = token.ValidTo
            //    });
            //}
            //return Unauthorized();
        }
    }
}
