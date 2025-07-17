using App.Common.Interfaces;
using App.Common.Security;
using App.Metrics.Commands.CreateMetric;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Metrics;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateMetricCommand NewMetric { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateMetricCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewMetric);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newMetricId = await mediator.Send(NewMetric);
        if (newMetricId > 0)
        {
            logger.LogInformation($"Created Metric with name {NewMetric.Name}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}