using App.Substations.Queries.GetSubstations;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Substations;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Substation> Substations { get; set; } = [];

    public async Task OnGetAsync()
    {
        Substations = await mediator.Send(new GetSubstationsQuery());
    }
}
