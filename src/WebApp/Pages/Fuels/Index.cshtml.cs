using App.Fuels.Queries.GetFuels;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Fuels;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Fuel> Fuels { get; set; } = [];

    public async Task OnGetAsync()
    {
        Fuels = await mediator.Send(new GetFuelsQuery());
    }
}
