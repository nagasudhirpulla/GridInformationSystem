using App.Bays.Commands.DeleteBay;
using App.Bays.Queries.GetBay;
using App.Common.Security;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Bays;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Bay Bay { get; set; }

    [BindProperty]
    public required DeleteBayCommand DelBayCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Bay = await mediator.Send(new GetBayQuery() { Id = id });
        DelBayCmd = new DeleteBayCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelBayCmd);
        logger.LogInformation($"Deleted Bay with id {DelBayCmd.Id}");
        return RedirectToPage("./Index");
    }
}