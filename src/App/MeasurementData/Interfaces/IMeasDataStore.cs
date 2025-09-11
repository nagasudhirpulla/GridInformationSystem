namespace App.MeasurementData.Interfaces;
public interface IMeasDataStore
{
    void InsertSamples(List<(int measId, int timestamp, float value)> samples);
    List<(int timestamp, float value)> FetchSamples(int measId, DateTime startTime, DateTime endTime);
    void EnsureDatabase();
}
