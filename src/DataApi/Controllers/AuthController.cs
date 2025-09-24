using App.ApiClients.Commands.GenerateJwtForApiClient;
using App.ApiClients.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace DataApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IMediator mediator, ILogger<MeasDataController> logger) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<JwtResponseDto> Login([FromBody] GenerateJwtForApiClientCommand cmd)
        {
            JwtSecurityToken? token = await mediator.Send(cmd) ?? throw new Exception("Unable to find user");

            logger.LogInformation($"Issuing Jwt for api client named {token.Subject}");

            return new JwtResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
    }
}
