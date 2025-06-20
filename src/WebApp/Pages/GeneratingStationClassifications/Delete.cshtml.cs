using App.Common.Security;
using App.GeneratingStationClassifications.Commands.DeleteGenStnClassification;
using App.GeneratingStationClassifications.Queries.GetGenStnClassification;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationClassifications;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required GeneratingStationClassification GeneratingStationClassification { get; set; }

    [BindProperty]
    public required DeleteGenStnClassificationCommand DelGeneratingStationClassificationCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        GeneratingStationClassification = await mediator.Send(new GetGenStnClassificationQuery() { Id = id });
        DelGeneratingStationClassificationCmd = new DeleteGenStnClassificationCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelGeneratingStationClassificationCmd);
        logger.LogInformation("Deleted GeneratingStationClassification with id {Id}", DelGeneratingStationClassificationCmd.Id);
        return RedirectToPage("./Index");
    }

}