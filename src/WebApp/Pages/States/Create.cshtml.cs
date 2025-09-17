using App.Common.Interfaces;
using App.Common.Security;
using App.Regions.Queries.GetRegions;
using App.States.Commands.CreateState;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.States;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateStateCommand NewState { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["RegionId"] = new SelectList(await mediator.Send(new GetRegionsQuery()), nameof(Region.Id), nameof(Region.Name));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateStateCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewState);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newStateId = await mediator.Send(NewState);
        if (newStateId > 0)
        {
            logger.LogInformation($"Created State with name {NewState.Name}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
