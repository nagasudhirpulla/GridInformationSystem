namespace Core.Entities.Data;

/// <summary>
/// Represent a datasource that fetches data from an external datasource
/// It is expected that the external datasource server follows a specific request and response schema
/// </summary>
public class ProxyDatasource : Datasource
{
    public required string BaseUrl { get; set; }
    public string? ApiKey { get; set; }

    // JSON schema of the request payload that will be sent to datasource
    public string? PayloadSchema { get; set; }
}