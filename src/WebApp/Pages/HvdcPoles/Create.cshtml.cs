using App.HvdcPoles.Commands.CreateHvdcPole;
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

namespace WebApp.Pages.HvdcPoles;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateHvdcPoleCommand NewHvdcPole { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.NameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewHvdcPole?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateHvdcPoleCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewHvdcPole);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newHvdcPoleId = await mediator.Send(NewHvdcPole);
        if (newHvdcPoleId > 0)
        {
            logger.LogInformation($"Created HvdcPole with Id {newHvdcPoleId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
