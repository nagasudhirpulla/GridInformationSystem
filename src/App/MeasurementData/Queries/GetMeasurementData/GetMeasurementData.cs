using App.Common.Security;
using App.MeasurementData.Dtos;
using App.MeasurementData.Interfaces;
using MediatR;

namespace App.MeasurementData.Queries.GetMeasurementData;

[Authorize]
public record GetMeasurementDataQuery : IRequest<List<MeasurementDataDto>>
{
    public required string MeasHistorianId { get; init; }
    public required string BaseUrl { get; set; }
    public string? ApiKey { get; set; }
    public string? JsonPayload { get; set; }
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
}

public class GetMeasurementDataQueryHandler(IProxyDataSourceFetcher fetcher) : IRequestHandler<GetMeasurementDataQuery, List<MeasurementDataDto>>
{
    public async Task<List<MeasurementDataDto>> Handle(GetMeasurementDataQuery request, CancellationToken cancellationToken)
    {
        var data = await fetcher.FetchData(request.MeasHistorianId, request.BaseUrl, request.ApiKey, request.JsonPayload, request.FromTime, request.ToTime);
        return data;
    }
}