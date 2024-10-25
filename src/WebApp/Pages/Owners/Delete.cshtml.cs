using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Commands.DeleteOwner;
using App.Owners.Queries.GetOwner;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Owners;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    public required Owner Owner { get; set; }

    [BindProperty]
    public required DeleteOwnerCommand DelOwnerCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Owner = await mediator.Send(new GetOwnerQuery() { Id = id });
        DelOwnerCmd = new DeleteOwnerCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelOwnerCmd);
        logger.LogInformation($"Deleted Owner with id {DelOwnerCmd.Id}");
        return RedirectToPage("./Index");
    }

}
