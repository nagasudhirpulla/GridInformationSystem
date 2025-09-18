using App.ApiRoles.Queries.GetApiRoles;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ApiRoles;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<ApiRole> ApiRoles { get; set; } = [];

    public async Task OnGetAsync()
    {
        ApiRoles = await mediator.Send(new GetApiRolesQuery());
    }
}
