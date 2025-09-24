// See https://aka.ms/new-console-template for more information
using DataApiClientDemo;

Console.WriteLine("Hello, World!");

var client = new DataClient(new HttpClient());
await client.InsertDataAsync(new InsertDataCommand() { Samples = [new InsertDataRecord() { MeasId = 1, Value = 1, Timestamp = 1 }] });