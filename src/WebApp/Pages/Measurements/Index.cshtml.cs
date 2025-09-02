using App.Measurements.Queries.GetMeasurements;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Measurements;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Measurement> Measurements { get; set; } = [];

    public async Task OnGetAsync()
    {
        Measurements = await mediator.Send(new GetMeasurementsQuery());
    }
}
