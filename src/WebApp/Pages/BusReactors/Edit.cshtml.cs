using App.BusReactors.Commands.UpdateBusReactor;
using App.BusReactors.Queries.GetBusReactor;
using App.Common.Interfaces;
using App.Owners.Queries.GetOwners;
using Core.Entities.Elements;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Common.Security;
using FluentValidation.AspNetCore;
using App.Buses.Queries.GetBuses;

namespace WebApp.Pages.BusReactors;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateBusReactorCommand BusReactor { get; set; }
    public async Task OnGetAsync(int id)
    {
        var busReactor = await mediator.Send(new GetBusReactorQuery() { Id = id });
        BusReactor = new UpdateBusReactorCommand()
        {
            Id = busReactor.Id,
            OwnerIds = string.Join(',', busReactor.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = busReactor.ElementNumber,
            CommissioningDate = busReactor.CommissioningDate,
            DeCommissioningDate = busReactor.DeCommissioningDate,
            CommercialOperationDate = busReactor.CommercialOperationDate,
            IsImportantGridElement = busReactor.IsImportantGridElement,
            BusId = busReactor.BusId,
            MvarCapacity = busReactor.MvarCapacity
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["BusId"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.ElementNameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), BusReactor.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateBusReactorCommandValidator(context);
        var validationResult = await validator.ValidateAsync(BusReactor);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(BusReactor);
        logger.LogInformation($"Updated BusReactor with {BusReactor.Id}");
        return RedirectToPage("./Index");
    }
}
