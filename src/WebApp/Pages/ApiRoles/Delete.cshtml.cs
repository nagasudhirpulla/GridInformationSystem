using App.ApiRoles.Commands.DeleteApiRole;
using App.ApiRoles.Queries.GetApiRole;
using App.Common.Security;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ApiRoles;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required ApiRole ApiRole { get; set; }

    [BindProperty]
    public required DeleteApiRoleCommand DelApiRoleCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        ApiRole = await mediator.Send(new GetApiRoleQuery() { Id = id });
        DelApiRoleCmd = new DeleteApiRoleCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelApiRoleCmd);
        logger.LogInformation("Deleted ApiRole with id {Id}", DelApiRoleCmd.Id);
        return RedirectToPage("./Index");
    }

}