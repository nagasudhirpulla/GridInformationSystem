using App.Common.Interfaces;
using App.Common.Security;
using App.Datasources.Queries.GetDatasources;
using App.Measurements.Commands.CreateMeasurement;
using App.Metrics.Queries.GetMetrics;
using Core.Entities.Data;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Measurements;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateMeasurementCommand NewMeasurement { get; set; }
    public async Task<IActionResult> OnGetAsync(int entityId)
    {
        if (entityId <= 0)
        {
            return NotFound();
        }
        NewMeasurement = new() { EntityId = entityId, MetricId = -1, HistorianPntId = "", DatasourceId = -1 };
        await InitSelectListsAsync();
        return Page();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["DatasourceId"] = new SelectList(await mediator.Send(new GetDatasourcesQuery()), nameof(Datasource.Id), nameof(Datasource.Name));
        ViewData["MetricId"] = new SelectList(await mediator.Send(new GetMetricsQuery()), nameof(Metric.Id), nameof(Metric.Name));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateMeasurementCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewMeasurement);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newMeasurementId = await mediator.Send(NewMeasurement);
        if (newMeasurementId > 0)
        {
            logger.LogInformation($"Created Measurement with Id {newMeasurementId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
