using App.GeneratingStations.Commands.CreateGeneratingStation;
using App.Common.Interfaces;
using App.Owners.Queries.GetOwners;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Common.Security;
using FluentValidation.AspNetCore;
using App.VoltageLevels.Queries.GetVoltageLevels;
using App.Locations.Queries.GetLocations;
using App.GeneratingStationClassifications.Queries.GetGenStnClassifications;
using App.GeneratingStationTypes.Queries.GetGenStationTypes;
using App.Fuels.Queries.GetFuels;

namespace WebApp.Pages.GeneratingStations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateGeneratingStationCommand NewGeneratingStation { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["VoltageLevelId"] = new SelectList(await mediator.Send(new GetVoltageLevelsQuery()), nameof(VoltageLevel.Id), nameof(VoltageLevel.Level));
        ViewData["LocationId"] = new SelectList(await mediator.Send(new GetLocationsQuery()), nameof(Location.Id), nameof(Location.Name));
        ViewData["FuelId"] = new SelectList(await mediator.Send(new GetFuelsQuery()), nameof(Fuel.Id), nameof(Fuel.FuelName));
        ViewData["GeneratingStationClassificationId"] = new SelectList(await mediator.Send(new GetGenStnClassificationsQuery()), nameof(GeneratingStationClassification.Id), nameof(GeneratingStationClassification.Classification));
        ViewData["GeneratingStationTypeId"] = new SelectList(await mediator.Send(new GetGenStationTypesQuery()), nameof(GeneratingStationType.Id), nameof(GeneratingStationType.StationType));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewGeneratingStation?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateGenStationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewGeneratingStation);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newGeneratingStationId = await mediator.Send(NewGeneratingStation);
        if (newGeneratingStationId > 0)
        {
            logger.LogInformation($"Created GeneratingStation with Id {newGeneratingStationId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}