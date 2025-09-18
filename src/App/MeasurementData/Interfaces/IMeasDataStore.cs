namespace App.MeasurementData.Interfaces;
public interface IMeasDataStore
{
    void InsertSamples(List<(int measId, int timestamp, float value)> samples);
    List<MeasurementDataDto> FetchSamples(int measId, DateTime startTime, DateTime endTime);
    void EnsureDatabase();
}
