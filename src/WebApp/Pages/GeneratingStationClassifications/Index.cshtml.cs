using App.GeneratingStationClassifications.Queries.GetGenStnClassifications;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationClassifications;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<GeneratingStationClassification> GeneratingStationClassifications { get; set; } = [];

    public async Task OnGetAsync()
    {
        GeneratingStationClassifications = await mediator.Send(new GetGenStnClassificationsQuery());
    }
}