using App.SubFilterBanks.Queries.GetSubFilterBanks;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.SubFilterBanks;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<SubFilterBank> SubFilterBanks { get; set; } = [];

    public async Task OnGetAsync()
    {
        SubFilterBanks = await mediator.Send(new GetSubFilterBanksQuery());
    }
}
