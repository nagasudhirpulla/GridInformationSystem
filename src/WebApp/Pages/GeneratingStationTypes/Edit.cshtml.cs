using App.Common.Interfaces;
using App.Common.Security;
using App.GeneratingStationTypes.Commands.UpdateGenStationType;
using App.GeneratingStationTypes.Queries.GetGenStationType;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationTypes;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateGenStationTypeCommand GeneratingStationType { get; set; }
    public async Task OnGetAsync(int id)
    {
        var region = await mediator.Send(new GetGenStationTypeQuery() { Id = id });
        GeneratingStationType = new UpdateGenStationTypeCommand() { Id = region.Id, StationType = region.StationType };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateGenStationTypeCommandValidator(context);
        var validationResult = await validator.ValidateAsync(GeneratingStationType);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(GeneratingStationType);
        logger.LogInformation($"Updated GeneratingStationType name to {GeneratingStationType.StationType}");
        return RedirectToPage("./Index");
    }
}