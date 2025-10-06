// See https://aka.ms/new-console-template for more information
using DataApiClientDemo;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .AddUserSecrets<ClientConfig>()
                 .AddEnvironmentVariables()
                 .Build();

DataClient client = new(new HttpClient());

ClientConfig? clientConfig = config.GetSection("DataClient").Get<ClientConfig>();

var tokenResp = await client.LoginAsync(clientConfig?.Key);

client = new(new HttpClient() { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", tokenResp?.Token) } });

await client.InsertDataAsync(new InsertDataCommand() { Samples = [new InsertDataRecord() { MeasId = 1, Value = 1, Timestamp = 1 }] });


public class ClientConfig
{
    public required string Id { get; set; }
    public required string Key { get; set; }
}