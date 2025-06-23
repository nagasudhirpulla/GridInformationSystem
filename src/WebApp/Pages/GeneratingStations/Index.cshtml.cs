using App.GeneratingStations.Queries.GetGeneratingStations;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStations;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<GeneratingStation> GeneratingStations { get; set; } = [];

    public async Task OnGetAsync()
    {
        GeneratingStations = await mediator.Send(new GetGeneratingStationsQuery());
    }
}
