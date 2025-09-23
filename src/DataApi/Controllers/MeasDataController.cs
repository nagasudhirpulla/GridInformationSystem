using App.Common.Security;
using App.MeasurementData.Commands.InsertData;
using App.MeasurementData.Queries.GetMeasurementData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MeasDataController(IMediator mediator, ILogger<MeasDataController> logger) : ControllerBase
    {
        private readonly ILogger<MeasDataController> _logger = logger;

        [HttpPost("GetData")]
        public async Task<List<(int timestamp, float value)>> Get([FromBody] GetMeasurementDataQuery cmd)
        {
            var data = await mediator.Send(cmd);
            return data;
        }

        [HttpPost("InsertData")]
        public async Task<IActionResult> Insert([FromBody] InsertDataCommand cmd)
        {
            await mediator.Send(cmd);
            return Ok();
        }
    }
}
