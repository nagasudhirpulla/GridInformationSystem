using App.Common.Security;
using App.Measurements.Commands.DeleteMeasurement;
using App.Measurements.Queries.GetMeasurement;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Measurements;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Measurement Measurement { get; set; }

    [BindProperty]
    public required DeleteMeasurementCommand DelMeasurementCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Measurement = await mediator.Send(new GetMeasurementQuery() { Id = id });
        DelMeasurementCmd = new DeleteMeasurementCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelMeasurementCmd);
        logger.LogInformation("Deleted Measurement with id {Id}", DelMeasurementCmd.Id);
        return RedirectToPage("./Index");
    }

}

