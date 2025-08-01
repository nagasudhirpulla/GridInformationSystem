using App.Common.Interfaces;
using App.Common.Security;
using App.Owners.Queries.GetOwners;
using App.Buses.Commands.CreateBus;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FluentValidation.AspNetCore;
using App.Substations.Queries.GetSubstations;

namespace WebApp.Pages.Buses;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateBusCommand NewBus { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewBus?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateBusCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewBus);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newBusId = await mediator.Send(NewBus);
        if (newBusId > 0)
        {
            logger.LogInformation($"Created Bus with Id {newBusId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
