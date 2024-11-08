using App.Buses.Queries.GetBuses;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Buses;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Bus> Buses { get; set; } = [];

    public async Task OnGetAsync()
    {
        Buses = await mediator.Send(new GetBusesQuery());
    }
}
