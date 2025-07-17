using App.Metrics.Queries.GetMetrics;
using Core.Entities.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Metrics;

public class IndexModel(IMediator mediator) : PageModel
{
    public IList<Metric> Metrics { get; set; } = [];

    public async Task OnGetAsync()
    {
        Metrics = await mediator.Send(new GetMetricsQuery());
    }
}