using App.Owners.Queries.GetOwners;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Owners;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Owner> Owners { get; set; } = [];

    public async Task OnGetAsync()
    {
        Owners = await mediator.Send(new GetOwnersQuery());
    }
}
