using App.Lines.Queries.GetLines;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Lines;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Line> Lines { get; set; } = [];

    public async Task OnGetAsync()
    {
        Lines = await mediator.Send(new GetLinesQuery());
    }
}
