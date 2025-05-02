using App.Common.Interfaces;
using App.Transformers.Commands.UpdateTransformer;
using App.Transformers.Queries.GetTransformer;
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
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateTransformerCommand Transformer { get; set; }
    public async Task OnGetAsync(int id)
    {
        var transformer = await mediator.Send(new GetTransformerQuery() { Id = id });
        Transformer = new UpdateTransformerCommand()
        {
            Id = transformer.Id,
            Substation1Id = transformer.Substation1Id,
            Substation2Id = transformer.Substation2Id??-1,
            OwnerIds = string.Join(',', transformer.ElementOwners.Select(x => x.OwnerId)),
            ElementNumber = transformer.ElementNumber,
            CommissioningDate = transformer.CommissioningDate,
            DeCommissioningDate = transformer.DeCommissioningDate,
            CommercialOperationDate = transformer.CommercialOperationDate,
            IsImportantGridElement = transformer.IsImportantGridElement,
            TransformerType = transformer.TransformerType,
            MvaCapacity = transformer.MvaCapacity,
            Make = transformer.Make
        };
        await InitSelectListsAsync();
    }

    private async Task InitSelectListsAsync()
    {
        ViewData["Substation1Id"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.NameCache));
        ViewData["Substation2Id"] = new SelectList(await mediator.Send(new GetSubstationsQuery()), nameof(Substation.Id), nameof(Substation.NameCache));
        ViewData["OwnerId"] = new MultiSelectList(await mediator.Send(new GetOwnersQuery()), nameof(Owner.Id), nameof(Owner.Name), Transformer?.OwnerIds.Split(","));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateTransformerCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Transformer);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            await InitSelectListsAsync();
            return Page();
        }

        await mediator.Send(Transformer);
        logger.LogInformation($"Updated Transformer with Id {Transformer.Id}");
        return RedirectToPage("./Index");
    }
}

