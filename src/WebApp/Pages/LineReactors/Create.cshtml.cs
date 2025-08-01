using App.Lines.Queries.GetLines;
using App.LineReactors.Commands.CreateLineReactor;
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
using App.Substations.Queries.GetSubstations;

namespace WebApp.Pages.LineReactors;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateLineReactorCommand NewLineReactor { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["LineId"] = new SelectList(await mediator.Send(new GetLinesQuery()), nameof(Line.Id), nameof(Line.Name));
        ViewData["SubstationId"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewLineReactor?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateLineReactorCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewLineReactor);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newLineReactorId = await mediator.Send(NewLineReactor);
        if (newLineReactorId > 0)
        {
            logger.LogInformation($"Created LineReactor with Id {newLineReactorId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}