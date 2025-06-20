using App.GeneratingStationTypes.Queries.GetGenStationTypes;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationTypes;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<GeneratingStationType> GeneratingStationTypes { get; set; } = [];

    public async Task OnGetAsync()
    {
        GeneratingStationTypes = await mediator.Send(new GetGenStationTypesQuery());
    }
}
