using App.MeasurementData.Interfaces;
using App.Utils;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Infra.Data;

public class ProxyDataSourceFetcher : IProxyDataSourceFetcher
{
    public async Task<List<(int timestamp, float value)>> FetchData(string measHistorianId, string baseUrl, string? apiKey, string? jsonPayload, DateTime fromTime, DateTime toTime)
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
                Query = $"fromTime={TimeUtils.GetUtcMs(fromTime)}&toTime={TimeUtils.GetUtcMs(fromTime)}&id={HttpUtility.UrlEncode(measHistorianId)}"
            };
            Uri requestUri = uriBuilder.Uri;

            // send request for data
            var postBody = new StringContent(jsonPayload ?? "{}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(requestUri, postBody);

            // parse response
            response.EnsureSuccessStatusCode(); // Throws an exception for non-success status codes
            string responseString = await response.Content.ReadAsStringAsync();
            var samples = JsonSerializer.Deserialize<List<(int timestamp, float value)>>(responseString);

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
