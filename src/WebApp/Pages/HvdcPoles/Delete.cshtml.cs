using App.HvdcPoles.Commands.DeleteHvdcPole;
using App.HvdcPoles.Queries.GetHvdcPole;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.HvdcPoles;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required HvdcPole HvdcPole { get; set; }

    [BindProperty]
    public required DeleteHvdcPoleCommand DelHvdcPoleCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        HvdcPole = await mediator.Send(new GetHvdcPoleQuery() { Id = id });
        DelHvdcPoleCmd = new DeleteHvdcPoleCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelHvdcPoleCmd);
        logger.LogInformation($"Deleted HvdcPole with id {DelHvdcPoleCmd.Id}");
        return RedirectToPage("./Index");
    }

}