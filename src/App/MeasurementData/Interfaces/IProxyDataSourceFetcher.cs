using App.MeasurementData.Dtos;

namespace App.MeasurementData.Interfaces;

public interface IProxyDataSourceFetcher
{
    public Task<List<MeasurementDataDto>> FetchData(string measHistorianId, string baseUrl, string? apiKey, string? jsonPayload, DateTime fromTime, DateTime toTime);
}
