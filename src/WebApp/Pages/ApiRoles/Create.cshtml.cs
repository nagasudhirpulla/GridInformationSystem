using App.ApiRoles.Commands.CreateApiRole;
using App.Common.Interfaces;
using App.Common.Security;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ApiRoles;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateApiRoleCommand NewApiRole { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateApiRoleCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewApiRole);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }


        var newApiRoleId = await mediator.Send(NewApiRole);
        if (newApiRoleId > 0)
        {
            logger.LogInformation($"Created ApiRole with name {NewApiRole.Name}");
            return RedirectToPage("./Index");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
