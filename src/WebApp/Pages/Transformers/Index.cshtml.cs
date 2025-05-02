using App.Transformers.Queries.GetTransformers;
using Core.Entities.Elements;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Transformers;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Transformer> Transformers { get; set; } = [];

    public async Task OnGetAsync()
    {
        Transformers = await mediator.Send(new GetTransformersQuery());
    }
}
