using App.Common.Security;
using App.Transformers.Commands.DeleteTransformer;
using App.Transformers.Queries.GetTransformer;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Transformers;

[Authorize(Roles = Core.Constants.Roles.Administrator)]
public class DeleteModel(ILogger<CreateModel> logger, IMediator mediator) : PageModel
{
    public required Transformer Transformer { get; set; }

    [BindProperty]
    public required DeleteTransformerCommand DelTransformerCmd { get; set; }

    public async Task OnGetAsync(int id)
    {
        Transformer = await mediator.Send(new GetTransformerQuery() { Id = id });
        DelTransformerCmd = new DeleteTransformerCommand(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(DelTransformerCmd);
        logger.LogInformation($"Deleted Transformer with id {DelTransformerCmd.Id}");
        return RedirectToPage("./Index");
    }

}
