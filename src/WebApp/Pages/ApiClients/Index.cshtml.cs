using App.ApiClients.Queries.GetApiClients;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ApiClients;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<ApiClient> ApiClients { get; set; } = [];

    public async Task OnGetAsync()
    {
        ApiClients = await mediator.Send(new GetApiClientsQuery());
    }
}
