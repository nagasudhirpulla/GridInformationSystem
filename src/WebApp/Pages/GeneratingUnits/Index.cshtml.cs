using App.GeneratingUnits.Queries.GetGeneratingUnits;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingUnits;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<GeneratingUnit> GeneratingUnits { get; set; } = [];

    public async Task OnGetAsync()
    {
        GeneratingUnits = await mediator.Send(new GetGeneratingUnitsQuery());
    }
}
