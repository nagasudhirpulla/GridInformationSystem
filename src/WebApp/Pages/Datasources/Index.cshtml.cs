using App.Datasources.Queries.GetDatasources;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Datasources;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Datasource> Datasources { get; set; } = [];

    public async Task OnGetAsync()
    {
        Datasources = await mediator.Send(new GetDatasourcesQuery());
    }
}
