using App.FilterBanks.Queries.GetFilterBanks;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.FilterBanks;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<FilterBank> FilterBanks { get; set; } = [];

    public async Task OnGetAsync()
    {
        FilterBanks = await mediator.Send(new GetFilterBanksQuery());
    }
}

