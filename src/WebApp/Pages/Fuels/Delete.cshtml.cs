using App.Common.Security;
using App.Fuels.Commands.DeleteFuel;
using App.Fuels.Queries.GetFuel;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Fuels;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Fuel Fuel { get; set; }

    [BindProperty]
    public required DeleteFuelCommand DelFuelCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Fuel = await mediator.Send(new GetFuelQuery() { Id = id });
        DelFuelCmd = new DeleteFuelCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelFuelCmd);
        logger.LogInformation("Deleted Fuel with id {Id}", DelFuelCmd.Id);
        return RedirectToPage("./Index");
    }

}