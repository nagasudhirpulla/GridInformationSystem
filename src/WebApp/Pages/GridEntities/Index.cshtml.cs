using App.GridEntities.Queries.GetGridEntities;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GridEntities;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<GridEntity> GridEntities { get; set; } = [];

    public async Task OnGetAsync()
    {
        GridEntities = await mediator.Send(new GetGridEntitiesQuery());
    }
}
