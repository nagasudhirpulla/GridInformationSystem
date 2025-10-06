using App.MeasurementData.Dtos;
using App.MeasurementData.Interfaces;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Infra.Data;

public class ProxyDataSourceFetcher : IProxyDataSourceFetcher
{
    public async Task<List<MeasurementDataDto>> FetchData(string measHistorianId, string baseUrl, string? apiKey, string? jsonPayload, int fromTimeUtcMs, int toTimeUtcMs)
    {
        try
        {
            using var client = new HttpClient();

            // use api key
            if (apiKey != null)
            {
                client.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);
            }

            // create full URI with query parameters
            UriBuilder uriBuilder = new(baseUrl)
            {
                Query = $"fromTime={fromTimeUtcMs}&toTime={toTimeUtcMs}&id={HttpUtility.UrlEncode(measHistorianId)}"
            };
            Uri requestUri = uriBuilder.Uri;

            // send request for data
            var postBody = new StringContent(jsonPayload ?? "{}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(requestUri, postBody);

            // parse response
            response.EnsureSuccessStatusCode(); // Throws an exception for non-success status codes
            string responseString = await response.Content.ReadAsStringAsync();
            var samples = JsonSerializer.Deserialize<List<MeasurementDataDto>>(responseString);

            // return samples
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
