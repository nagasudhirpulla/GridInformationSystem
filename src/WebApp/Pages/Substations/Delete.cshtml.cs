using App.Common.Interfaces;
using App.Common.Security;
using App.Substations.Commands.DeleteSubstation;
using App.Substations.Queries.GetSubstation;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Substations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    public required Substation Substation { get; set; }

    [BindProperty]
    public required DeleteSubstationCommand DelSubstationCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Substation = await mediator.Send(new GetSubstationQuery() { Id = id });
        DelSubstationCmd = new DeleteSubstationCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelSubstationCmd);
        logger.LogInformation($"Deleted Substation with id {DelSubstationCmd.Id}");
        return RedirectToPage("./Index");
    }

}