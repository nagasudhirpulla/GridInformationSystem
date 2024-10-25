using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Commands.CreateOwner;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Owners;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateOwnerCommand NewOwner { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateOwnerCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewOwner);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newOwnerId = await mediator.Send(NewOwner);
        if (newOwnerId > 0)
        {
            logger.LogInformation($"Created Owner with name {NewOwner.Name}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}