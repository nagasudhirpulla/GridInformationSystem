using App.MeasurementData.Commands.InsertData;
using App.MeasurementData.Dtos;

namespace App.MeasurementData.Interfaces;
public interface IMeasDataStore
{
    // TODO return number of samples instead
    Task InsertSamples(List<InsertDataRecord> samples);
    List<MeasurementDataDto> FetchSamples(int measId, DateTime startTime, DateTime endTime);
    void EnsureDatabase();
}
