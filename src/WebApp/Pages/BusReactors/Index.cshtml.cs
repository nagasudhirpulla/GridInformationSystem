using App.BusReactors.Queries.GetBusReactors;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.BusReactors;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<BusReactor> BusReactors { get; set; } = [];

    public async Task OnGetAsync()
    {
        BusReactors = await mediator.Send(new GetBusReactorsQuery());
    }
}