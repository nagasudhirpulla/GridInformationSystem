using App.Common.Security;
using App.Metrics.Commands.DeleteMetric;
using App.Metrics.Queries.GetMetric;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Metrics;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Metric Metric { get; set; }

    [BindProperty]
    public required DeleteMetricCommand DelMetricCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Metric = await mediator.Send(new GetMetricQuery() { Id = id });
        DelMetricCmd = new DeleteMetricCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelMetricCmd);
        logger.LogInformation("Deleted Metric with id {Id}", DelMetricCmd.Id);
        return RedirectToPage("./Index");
    }

}
