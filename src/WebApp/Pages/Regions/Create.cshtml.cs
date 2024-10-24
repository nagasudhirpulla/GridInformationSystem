using App.Common.Interfaces;
using App.Common.Security;
using App.Regions.Commands.CreateRegion;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FluentValidation.AspNetCore;

namespace WebApp.Pages.Regions;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateRegionCommand NewRegion { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateRegionCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewRegion);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newRegionId = await mediator.Send(NewRegion);
        if (newRegionId > 0)
        {
            logger.LogInformation($"Created Region with name {NewRegion.Name}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}