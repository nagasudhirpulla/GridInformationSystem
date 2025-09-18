using App.ApiClients.Commands.UpdateApiClient;
using App.ApiClients.Queries.GetApiClient;
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
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateApiClientCommand ApiClient { get; set; }
    public async Task OnGetAsync(int id)
    {
        var apiClient = await mediator.Send(new GetApiClientQuery() { Id = id });
        ApiClient = new UpdateApiClientCommand()
        {
            Id = apiClient.Id,
            Key = apiClient.Key,
            Name = apiClient.Name,
            ApiRoleIds = string.Join(',', apiClient.ApiClientRoles.Select(x => x.ApiRoleId))
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["ApiRoleId"] = new MultiSelectList(await mediator.Send(new GetApiRolesQuery()), nameof(ApiRole.Id), nameof(ApiRole.Name), ApiClient.ApiRoleIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateApiClientCommandValidator(context);
        var validationResult = await validator.ValidateAsync(ApiClient);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(ApiClient);
        logger.LogInformation($"Updated ApiClient with {ApiClient.Id}");
        return RedirectToPage("./Index");
    }
}
