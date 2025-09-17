using App.Common.Security;
using App.GeneratingUnits.Commands.DeleteGeneratingUnit;
using App.GeneratingUnits.Queries.GetGeneratingUnit;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingUnits;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required GeneratingUnit GeneratingUnit { get; set; }

    [BindProperty]
    public required DeleteGeneratingUnitCommand DelGeneratingUnitCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        GeneratingUnit = await mediator.Send(new GetGeneratingUnitQuery() { Id = id });
        DelGeneratingUnitCmd = new DeleteGeneratingUnitCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelGeneratingUnitCmd);
        logger.LogInformation($"Deleted GeneratingUnit with id {DelGeneratingUnitCmd.Id}");
        return RedirectToPage("./Index");
    }

}
