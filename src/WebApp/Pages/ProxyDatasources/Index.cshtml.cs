using App.ProxyDatasources.Queries.GetProxyDatasources;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ProxyDatasources;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<ProxyDatasource> ProxyDatasources { get; set; } = [];

    public async Task OnGetAsync()
    {
        ProxyDatasources = await mediator.Send(new GetProxyDatasourcesQuery());
    }
}

