using App.LineReactors.Queries.GetLineReactors;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.LineReactors;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<LineReactor> LineReactors { get; set; } = [];

    public async Task OnGetAsync()
    {
        LineReactors = await mediator.Send(new GetLineReactorsQuery());
    }
}