using App.GeneratingUnits.Commands.CreateGeneratingUnit;
using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Queries.GetOwners;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.GeneratingStations.Queries.GetGeneratingStations;

namespace WebApp.Pages.GeneratingUnits;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateGeneratingUnitCommand NewGeneratingUnit { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetGeneratingStationsQuery()), nameof(GeneratingStation.Id), nameof(GeneratingStation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewGeneratingUnit?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateGeneratingUnitCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewGeneratingUnit);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newGeneratingUnitId = await mediator.Send(NewGeneratingUnit);
        if (newGeneratingUnitId > 0)
        {
            logger.LogInformation($"Created GeneratingUnit with Id {newGeneratingUnitId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}

