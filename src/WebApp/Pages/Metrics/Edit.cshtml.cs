using App.Common.Interfaces;
using App.Common.Security;
using App.Metrics.Commands.UpdateMetric;
using App.Metrics.Queries.GetMetric;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Metrics;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateMetricCommand Metric { get; set; }
    public async Task OnGetAsync(int id)
    {
        var metric = await mediator.Send(new GetMetricQuery() { Id = id });
        Metric = new UpdateMetricCommand() { Id = metric.Id, Name = metric.Name, Unit = metric.Unit };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateMetricCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Metric);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(Metric);
        logger.LogInformation($"Updated Metric name to {Metric.Name}");
        return RedirectToPage("./Index");
    }
}