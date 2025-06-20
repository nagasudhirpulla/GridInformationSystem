using App.Common.Interfaces;
using App.Common.Security;
using App.GeneratingStationClassifications.Commands.CreateGenStnClassification;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationClassifications;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateGenStnClassificationCommand NewGeneratingStationClassification { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateGenStnClassificationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewGeneratingStationClassification);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newGeneratingStationClassificationId = await mediator.Send(NewGeneratingStationClassification);
        if (newGeneratingStationClassificationId > 0)
        {
            logger.LogInformation($"Created GeneratingStationClassification with name {NewGeneratingStationClassification.Classification}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
