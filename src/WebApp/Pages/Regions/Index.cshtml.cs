using App.Regions.Queries.GetRegions;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Regions;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Region> Regions { get; set; } = [];

    public async Task OnGetAsync()
    {
        Regions = await mediator.Send(new GetRegionsQuery());
    }
}
