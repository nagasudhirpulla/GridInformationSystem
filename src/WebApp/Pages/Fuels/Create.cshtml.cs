using App.Common.Interfaces;
using App.Common.Security;
using App.Fuels.Commands.CreateFuel;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Fuels;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateFuelCommand NewFuel { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateFuelCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewFuel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newFuelId = await mediator.Send(NewFuel);
        if (newFuelId > 0)
        {
            logger.LogInformation($"Created Fuel with name {NewFuel.Name}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}