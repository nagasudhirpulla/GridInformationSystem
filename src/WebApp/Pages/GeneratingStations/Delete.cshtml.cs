using App.Common.Security;
using App.GeneratingStations.Commands.DeleteGeneratingStation;
using App.GeneratingStations.Queries.GetGeneratingStation;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required GeneratingStation GeneratingStation { get; set; }

    [BindProperty]
    public required DeleteGeneratingStationCommand DelGeneratingStationCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        GeneratingStation = await mediator.Send(new GetGeneratingStationQuery() { Id = id });
        DelGeneratingStationCmd = new DeleteGeneratingStationCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelGeneratingStationCmd);
        logger.LogInformation($"Deleted GeneratingStation with id {DelGeneratingStationCmd.Id}");
        return RedirectToPage("./Index");
    }

}
