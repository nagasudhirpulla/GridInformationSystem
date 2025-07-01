using App.Buses.Queries.GetBuses;
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
using App.HvdcLines.Commands.UpdateHvdcLine;
using App.HvdcLines.Queries.GetHvdcLine;

namespace WebApp.Pages.HvdcLines;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateHvdcLineCommand HvdcLine { get; set; }
    public async Task OnGetAsync(int id)
    {
        var line = await mediator.Send(new GetHvdcLineQuery() { Id = id });
        HvdcLine = new UpdateHvdcLineCommand()
        {
            Id = line.Id,
            Bus1Id = line.Bus1Id,
            Bus2Id = line.Bus2Id,
            OwnerIds = string.Join(',', line.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = line.ElementNumber,
            CommissioningDate = line.CommissioningDate,
            DeCommissioningDate = line.DeCommissioningDate,
            CommercialOperationDate = line.CommercialOperationDate,
            IsImportantGridElement = line.IsImportantGridElement,
            Length = line.Length,
            ConductorType = line.ConductorType,
            IsSpsPresent = line.IsSpsPresent
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["Bus1Id"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.ElementNameCache));
        ViewData["Bus2Id"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.ElementNameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), HvdcLine.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateHvdcLineCommandValidator(context);
        var validationResult = await validator.ValidateAsync(HvdcLine);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(HvdcLine);
        logger.LogInformation($"Created HvdcLine with Id {HvdcLine.Id}");
        return RedirectToPage("./Index");
    }
}

