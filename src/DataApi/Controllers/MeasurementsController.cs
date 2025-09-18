using App.MeasurementData.Queries.GetMeasurementData;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasurementsController(ILogger<MeasurementsController> logger) : ControllerBase
    {
        private readonly ILogger<MeasurementsController> _logger = logger;

        [HttpPost(Name = "GetData")]
        public IEnumerable<int> Get([FromBody] GetMeasurementDataQuery cmd)
        {
            return [.. Enumerable.Range(1, 5)];
        }
    }
}
