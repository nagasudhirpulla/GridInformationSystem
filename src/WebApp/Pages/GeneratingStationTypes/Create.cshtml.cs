using App.Common.Interfaces;
using App.Common.Security;
using App.GeneratingStationTypes.Commands.CreateGenStationType;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationTypes;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateGenStationTypeCommand NewGeneratingStationType { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateGenStationTypeCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewGeneratingStationType);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newGeneratingStationTypeId = await mediator.Send(NewGeneratingStationType);
        if (newGeneratingStationTypeId > 0)
        {
            logger.LogInformation($"Created GeneratingStationType with name {NewGeneratingStationType.StationType}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}