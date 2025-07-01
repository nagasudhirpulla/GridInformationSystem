using App.HvdcLines.Queries.GetHvdcLines;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.HvdcLines;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<HvdcLine> HvdcLines { get; set; } = [];

    public async Task OnGetAsync()
    {
        HvdcLines = await mediator.Send(new GetHvdcLinesQuery());
    }
}
