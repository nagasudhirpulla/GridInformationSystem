using App.HvdcPoles.Queries.GetHvdcPoles;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.HvdcPoles;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<HvdcPole> HvdcPoles { get; set; } = [];

    public async Task OnGetAsync()
    {
        HvdcPoles = await mediator.Send(new GetHvdcPolesQuery());
    }
}

