using App.Lines.Commands.UpdateLine;
using App.Owners.Queries.GetOwners;
using Core.Entities.Elements;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Common.Security;
using MediatR;
using App.Common.Interfaces;
using FluentValidation.AspNetCore;
using App.Lines.Queries.GetLine;
using App.Buses.Queries.GetBuses;

namespace WebApp.Pages.Lines;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateLineCommand Line { get; set; }
    public async Task OnGetAsync(int id)
    {
        var line = await mediator.Send(new GetLineQuery() { Id = id });
        Line = new UpdateLineCommand()
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
            IsAutoReclosurePresent = line.IsAutoReclosurePresent
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["Bus1Id"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.Name));
        ViewData["Bus2Id"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), Line.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateLineCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Line);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(Line);
        logger.LogInformation($"Created Line with Id {Line.Id}");
        return RedirectToPage("./Index");
    }
}
