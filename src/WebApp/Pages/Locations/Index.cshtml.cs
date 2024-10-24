using App.Locations.Queries.GetLocations;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Locations;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Location> Locations { get; set; } = [];

    public async Task OnGetAsync()
    {
        Locations = await mediator.Send(new GetLocationsQuery());
    }
}