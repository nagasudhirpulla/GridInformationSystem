using App.Buses.Queries.GetBuses;
using App.LineReactors.Commands.UpdateLineReactor;
using App.LineReactors.Queries.GetLineReactor;
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
using App.Lines.Queries.GetLines;

namespace WebApp.Pages.LineReactors;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<EditModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateLineReactorCommand LineReactor { get; set; }
    public async Task OnGetAsync(int id)
    {
        var lineReactor = await mediator.Send(new GetLineReactorQuery() { Id = id });
        LineReactor = new UpdateLineReactorCommand()
        {
            Id = lineReactor.Id,
            OwnerIds = string.Join(',', lineReactor.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = lineReactor.ElementNumber,
            CommissioningDate = lineReactor.CommissioningDate,
            DeCommissioningDate = lineReactor.DeCommissioningDate,
            CommercialOperationDate = lineReactor.CommercialOperationDate,
            IsImportantGridElement = lineReactor.IsImportantGridElement,
            SubstationId = lineReactor.Substation1Id,
            LineId = lineReactor.LineId,
            MvarCapacity = lineReactor.MvarCapacity,
            IsConvertible = lineReactor.IsConvertible,
            IsSwitchable = lineReactor.IsSwitchable
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["LineId"] = new SelectList(await mediator.Send(new GetLinesQuery()), nameof(Line.Id), nameof(Line.ElementNameCache));
        ViewData["BusId"] = new SelectList(await mediator.Send(new GetBusesQuery()), nameof(Bus.Id), nameof(Bus.ElementNameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), LineReactor.OwnerIds.Split(','));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateLineReactorCommandValidator(context);
        var validationResult = await validator.ValidateAsync(LineReactor);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(LineReactor);
        logger.LogInformation($"Updated LineReactor with {LineReactor.Id}");
        return RedirectToPage("./Index");
    }
}

