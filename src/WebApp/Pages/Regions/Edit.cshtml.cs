using App.Common.Interfaces;
using App.Common.Security;
using App.Regions.Queries.GetRegions;
using App.Regions.Commands.UpdateRegion;
using App.Regions.Queries.GetRegion;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Regions;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class EditModel(ILogger<CreateModel> logger, IMediator mediator, IApplicationDbContext context) : PageModel
{
    [BindProperty]
    public required UpdateRegionCommand Region { get; set; }
    public async Task OnGetAsync(int id)
    {
        var region = await mediator.Send(new GetRegionQuery() { Id = id });
        Region = new UpdateRegionCommand() { Id = region.Id, Name = region.Name };
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var validator = new UpdateRegionCommandValidator(context);
        var validationResult = await validator.ValidateAsync(Region);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, null);
            return Page();
        }

        await mediator.Send(Region);
        logger.LogInformation($"Updated Region name to {Region.Name}");
        return RedirectToPage("./Index");
    }
}