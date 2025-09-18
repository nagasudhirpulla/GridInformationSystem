using App.ApiClients.Commands.CreateApiClient;
using App.ApiRoles.Queries.GetApiRoles;
using App.Common.Interfaces;
using App.Common.Security;
using Core.Entities.Data;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.ApiClients;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateApiClientCommand NewApiClient { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["ApiRoleId"] = new MultiSelectList(await mediator.Send(new GetApiRolesQuery()), nameof(ApiRole.Id), nameof(ApiRole.Name), NewApiClient?.ApiRoleIds.Split(','));

    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateApiClientCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewApiClient);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newApiClientId = await mediator.Send(NewApiClient);
        if (newApiClientId > 0)
        {
            logger.LogInformation($"Created ApiClient with substationId {newApiClientId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
