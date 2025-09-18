using App.Common.Interfaces;
using App.Common.Security;
using App.MeasurementData.Interfaces;
using Core.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.MeasurementData.Queries.GetMeasurementData;

[Authorize]
public record GetMeasurementDataQuery : IRequest<List<MeasurementDataDto>>
{
    public int MeasId { get; init; }
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
    public string? JsonPayload { get; set; }
}

public class GetMeasurementDataQueryHandler(IProxyDataSourceFetcher fetcher, IMeasDataStore measDataStore, IApplicationDbContext context) : IRequestHandler<GetMeasurementDataQuery, List<MeasurementDataDto>>
{
    public async Task<List<MeasurementDataDto>> Handle(GetMeasurementDataQuery request, CancellationToken cancellationToken)
    {
        // get the measurement
        Measurement meas = await context.Measurements.Where(m => m.Id == request.MeasId)
                                .Include(m => m.Datasource)
                                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new KeyNotFoundException();
        List<MeasurementDataDto> data = [];
        if (meas.Datasource is ProxyDatasource pds)
        {
            // TODO validate payload schema if required
            data = await fetcher.FetchData(meas.HistorianPntId, pds.BaseUrl, pds.ApiKey, request.JsonPayload, request.FromTime, request.ToTime);
        }
        else
        {
            data = measDataStore.FetchSamples(meas.Id, request.FromTime, request.ToTime);
        }
        return data;
    }
}