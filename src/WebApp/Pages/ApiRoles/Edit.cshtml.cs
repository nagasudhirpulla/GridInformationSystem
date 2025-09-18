using App.ApiRoles.Commands.UpdateApiRole;
using App.ApiRoles.Queries.GetApiRole;
using App.Common.Interfaces;
using App.Common.Security;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ApiRoles;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateApiRoleCommand ApiRole { get; set; }
    public async Task OnGetAsync(int id)
    {
        var role = await mediator.Send(new GetApiRoleQuery() { Id = id });
        ApiRole = new UpdateApiRoleCommand() { Id = role.Id, Name = role.Name };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateApiRoleCommandValidator(context);
        var validationResult = await validator.ValidateAsync(ApiRole);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(ApiRole);
        logger.LogInformation($"Updated ApiRole name to {ApiRole.Name}");
        return RedirectToPage("./Index");
    }
}