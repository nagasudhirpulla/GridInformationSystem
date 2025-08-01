using App.Common.Interfaces;
using App.Common.Security;
using App.Locations.Queries.GetLocations;
using App.Owners.Queries.GetOwners;
using App.GeneratingStations.Commands.UpdateGeneratingStation;
using App.GeneratingStations.Queries.GetGeneratingStation;
using App.VoltageLevels.Queries.GetVoltageLevels;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Fuels.Queries.GetFuels;
using App.GeneratingStationClassifications.Queries.GetGenStnClassifications;
using App.GeneratingStationTypes.Queries.GetGenStationTypes;
using FluentValidation.AspNetCore;

namespace WebApp.Pages.GeneratingStations;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateGeneratingStationCommand GeneratingStation { get; set; }
    public async Task OnGetAsync(int id)
    {
        var genStn = await mediator.Send(new GetGeneratingStationQuery() { Id = id });
        GeneratingStation = new UpdateGeneratingStationCommand()
        {
            Name = genStn.Name,
            Id = genStn.Id,
            VoltageLevelId = genStn.VoltageLevelId,
            IsAc = genStn.IsAc,
            LocationId = genStn.LocationId,
            Latitude = genStn.Latitude,
            Longitude = genStn.Longitude,
            InstalledCapacity = genStn.InstalledCapacity,
            MvaCapacity = genStn.MvaCapacity,
            GeneratingStationClassificationId = genStn.GeneratingStationClassificationId,
            GeneratingStationTypeId = genStn.GeneratingStationTypeId,
            OwnerIds = string.Join(',', genStn.SubstationOwners.Select(x => x.OwnerId))
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["LocationId"] = new SelectList(await mediator.Send(new GetLocationsQuery()), nameof(Location.Id), nameof(Location.Name));
        ViewData["VoltageLevelId"] = new SelectList(await mediator.Send(new GetVoltageLevelsQuery()), nameof(VoltageLevel.Id), nameof(VoltageLevel.Level));
        ViewData["FuelId"] = new SelectList(await mediator.Send(new GetFuelsQuery()), nameof(Fuel.Id), nameof(Fuel.Name));
        ViewData["GeneratingStationClassificationId"] = new SelectList(await mediator.Send(new GetGenStnClassificationsQuery()), nameof(GeneratingStationClassification.Id), nameof(GeneratingStationClassification.Classification));
        ViewData["GeneratingStationTypeId"] = new SelectList(await mediator.Send(new GetGenStationTypesQuery()), nameof(GeneratingStationType.Id), nameof(GeneratingStationType.StationType));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), GeneratingStation.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateGeneratingStationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(GeneratingStation);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(GeneratingStation);
        logger.LogInformation($"Updated GeneratingStation with {GeneratingStation.Id}");
        return RedirectToPage("./Index");
    }
}
