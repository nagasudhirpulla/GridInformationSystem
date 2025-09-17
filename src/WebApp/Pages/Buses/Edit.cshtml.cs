using App.Buses.Commands.UpdateBus;
using App.Buses.Queries.GetBus;
using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Queries.GetOwners;
using App.Substations.Queries.GetSubstations;
using Core.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Buses;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateBusCommand Bus { get; set; }
    public async Task OnGetAsync(int id)
    {
        var bus = await mediator.Send(new GetBusQuery() { Id = id });
        Bus = new UpdateBusCommand()
        {
            Id = bus.Id,
            SubstationId = bus.Substation1Id,
            OwnerIds = string.Join(',', bus.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = bus.ElementNumber,
            CommissioningDate = bus.CommissioningDate,
            DeCommissioningDate = bus.DeCommissioningDate,
            CommercialOperationDate = bus.CommercialOperationDate,
            IsImportantGridElement = bus.IsImportantGridElement,
            BusType = bus.BusType
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), Bus.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateBusCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Bus);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(Bus);
        logger.LogInformation($"Updated Bus with {Bus.Id}");
        return RedirectToPage("./Index");
    }
}