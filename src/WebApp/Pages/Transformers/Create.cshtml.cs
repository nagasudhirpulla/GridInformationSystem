using App.Common.Interfaces;
using App.Transformers.Commands.CreateTransformer;
using App.Owners.Queries.GetOwners;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Common.Security;
using FluentValidation.AspNetCore;
using App.Substations.Queries.GetSubstations;

namespace WebApp.Pages.Transformers;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class CreateModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required CreateTransformerCommand NewTransformer { get; set; }
    public async Task OnGetAsync()
    {
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["Substation1Id"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.Name));
        ViewData["Substation2Id"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.Name));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), NewTransformer?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new CreateTransformerCommandValidator(context);
        var validationResult = await validator.ValidateAsync(NewTransformer);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }


        var newTransformerId = await mediator.Send(NewTransformer);
        if (newTransformerId > 0)
        {
            logger.LogInformation($"Created Transformer with Id {newTransformerId}");
            return RedirectToPage("./Index");
        }

        await InitSelectListsAsync();
        // If we got this far, something failed, redisplay form
        return Page();
    }
}
