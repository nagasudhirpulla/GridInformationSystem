using App.Common.Interfaces;
using App.Common.Security;
using App.VoltageLevels.Commands.DeleteVoltageLevel;
using App.VoltageLevels.Queries.GetVoltageLevel;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.VoltageLevels;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required VoltageLevel VoltageLevel { get; set; }

    [BindProperty]
    public required DeleteVoltageLevelCommand DelVoltageLevelCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        VoltageLevel = await mediator.Send(new GetVoltageLevelQuery() { Id = id });
        DelVoltageLevelCmd = new DeleteVoltageLevelCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelVoltageLevelCmd);
        logger.LogInformation("Deleted VoltageLevel with id {Id}", DelVoltageLevelCmd.Id);
        return RedirectToPage("./Index");
    }

}
