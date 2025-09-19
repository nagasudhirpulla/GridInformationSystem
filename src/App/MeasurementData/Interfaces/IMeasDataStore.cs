namespace App.MeasurementData.Interfaces;
public interface IMeasDataStore
{
    // TODO return number of samples instead
    Task InsertSamples(List<(int measId, int timestamp, float value)> samples);
    List<MeasurementDataDto> FetchSamples(int measId, DateTime startTime, DateTime endTime);
    void EnsureDatabase();
}
