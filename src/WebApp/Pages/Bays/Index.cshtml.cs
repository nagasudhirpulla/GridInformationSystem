using App.Bays.Queries.GetBays;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Bays;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Bay> Bays { get; set; } = [];

    public async Task OnGetAsync()
    {
        Bays = await mediator.Send(new GetBaysQuery());
    }
}
