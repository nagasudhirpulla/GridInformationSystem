using App.States.Queries.GetStates;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.States;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<State> States { get; set; } = [];

    public async Task OnGetAsync()
    {
        States = await mediator.Send(new GetStatesQuery());
    }
}
