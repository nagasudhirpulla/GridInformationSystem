using App.Common.Interfaces;
using App.Common.Security;
using App.GeneratingStationClassifications.Commands.UpdateGenStnClassification;
using App.GeneratingStationClassifications.Queries.GetGenStnClassification;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GeneratingStationClassifications;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateGenStnClassificationCommand GeneratingStationClassification { get; set; }
    public async Task OnGetAsync(int id)
    {
        var genStnClassification = await mediator.Send(new GetGenStnClassificationQuery() { Id = id });
        GeneratingStationClassification = new UpdateGenStnClassificationCommand() { Id = genStnClassification.Id, Classification = genStnClassification.Classification };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateGenStnClassificationCommandValidator(context);
        var validationResult = await validator.ValidateAsync(GeneratingStationClassification);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(GeneratingStationClassification);
        logger.LogInformation($"Updated GeneratingStationClassification name to {GeneratingStationClassification.Classification}");
        return RedirectToPage("./Index");
    }
}
