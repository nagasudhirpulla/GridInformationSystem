using App.VoltageLevels.Queries.GetVoltageLevels;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.VoltageLevels;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<VoltageLevel> VoltageLevels { get; set; } = [];

    public async Task OnGetAsync()
    {
        VoltageLevels = await mediator.Send(new GetVoltageLevelsQuery());
    }
}
