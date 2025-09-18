global using MeasurementDataDto = (int timestamp, float value);
namespace App.MeasurementData.Interfaces;
public interface IProxyDataSourceFetcher
{
    public Task<List<MeasurementDataDto>> FetchData(string measHistorianId, string baseUrl, string? apiKey, string? jsonPayload, DateTime fromTime, DateTime toTime);
}
