using App.Common.Security;
using App.GeneratingStationTypes.Commands.DeleteGenStationType;
using App.GeneratingStationTypes.Queries.GetGenStationType;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationTypes;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required GeneratingStationType GeneratingStationType { get; set; }

    [BindProperty]
    public required DeleteGenStationTypeCommand DelGeneratingStationTypeCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        GeneratingStationType = await mediator.Send(new GetGenStationTypeQuery() { Id = id });
        DelGeneratingStationTypeCmd = new DeleteGenStationTypeCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelGeneratingStationTypeCmd);
        logger.LogInformation("Deleted GeneratingStationType with id {Id}", DelGeneratingStationTypeCmd.Id);
        return RedirectToPage("./Index");
    }

}