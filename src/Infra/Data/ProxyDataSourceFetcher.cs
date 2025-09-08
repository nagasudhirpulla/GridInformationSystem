using App.MeasurementData.Dtos;
using App.MeasurementData.Interfaces;
using System;
using System.Text;
using System.Text.Json;

namespace Infra.Data;

public class ProxyDataSourceFetcher : IProxyDataSourceFetcher
{
    public async Task<List<MeasurementDataDto>> FetchData(string measHistorianId, string baseUrl, string? apiKey, string? jsonPayload, DateTime fromTime, DateTime toTime)
    {
        try
        {
            using var client = new HttpClient();
            // TODO use times, measId, api key  in payload
            var postBody = new StringContent(jsonPayload ?? "{}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(baseUrl, postBody);
            response.EnsureSuccessStatusCode(); // Throws an exception for non-success status codes
            // TODO optimize this
            string responseBody = await response.Content.ReadAsStringAsync();
            var samples = JsonSerializer.Deserialize<List<MeasurementDataDto>>(responseBody);
            return samples ?? [];
        }
        catch (HttpRequestException)
        {
            //Console.WriteLine($"Request error: {e.Message}");
            return [];
        }
        catch (Exception)
        {
            //Console.WriteLine($"An unexpected error occurred: {e.Message}");
            return [];
        }
    }
}
