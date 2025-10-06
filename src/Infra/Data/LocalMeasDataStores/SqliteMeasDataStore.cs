using App.MeasurementData.Commands.InsertData;
using App.MeasurementData.Dtos;
using App.MeasurementData.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infra.Data.LocalMeasDataStores;

public class SqliteMeasDataStore(IConfiguration configuration) : IMeasDataStore
{
    private readonly string? DbConnStr = configuration.GetConnectionString("MeasDataConnectionString");
    private readonly string MeasDataTableName = "MeasData";
    private readonly string TimeColName = "MeasData";
    private readonly string ValColName = "MeasData";
    private readonly string MeasIdColName = "MeasData";

    public void EnsureDatabase()
    {
        using var db = new SqliteConnection(DbConnStr);
        db.Open();
        string createTableCmdStr = $@"CREATE TABLE IF NOT EXISTS {MeasDataTableName} (
	                                    {TimeColName} INTEGER NOT NULL,
	                                    {ValColName} REAL NOT NULL,
	                                    {MeasIdColName} INTEGER NOT NULL,
	                                    CONSTRAINT MeasData_UN UNIQUE ({TimeColName},{MeasIdColName})
                                    );";
        var createTableCmd = new SqliteCommand(createTableCmdStr, db);
        createTableCmd.ExecuteReader();
    }

    public List<MeasurementDataDto> FetchSamples(int measId, int startTimeUtcMs, int endTimeUtcMs)
    {
        List<MeasurementDataDto> samples = [];
        using (var db = new SqliteConnection(DbConnStr))
        {
            db.Open();
            var selectCommand = new SqliteCommand($@"SELECT {TimeColName},{ValColName} from {MeasDataTableName} 
                                                    where ({MeasIdColName}=@measId) 
                                                    and ({TimeColName} BETWEEN @startTime and @endTime);", db);
            selectCommand.Parameters.AddWithValue("@measId", measId);
            selectCommand.Parameters.AddWithValue("@startTime", startTimeUtcMs);
            selectCommand.Parameters.AddWithValue("@endTime", endTimeUtcMs);

            using var reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                int dt = reader.GetInt32(reader.GetOrdinal(TimeColName));
                float val = reader.GetFloat(reader.GetOrdinal(ValColName));
                samples.Add(new MeasurementDataDto() { Timestamp = dt, Value = val });
            }
        }
        return samples;
    }

    public async Task InsertSamples(List<InsertDataRecord> samples)
    {
        using var db = new SqliteConnection(DbConnStr);
        db.Open();
        var upsertCommand = new SqliteCommand($@"INSERT INTO {MeasDataTableName} ({TimeColName}, {MeasIdColName}, {ValColName})
                                                VALUES (@timestamp, @measId, @value)
                                                ON CONFLICT ({TimeColName}, {MeasIdColName}) DO UPDATE SET
                                                    {ValColName} = excluded.{ValColName};", db);
        // Add parameters once
        upsertCommand.Parameters.Add(new SqliteParameter("@timestamp", SqliteType.Integer));
        upsertCommand.Parameters.Add(new SqliteParameter("@measId", SqliteType.Integer));
        upsertCommand.Parameters.Add(new SqliteParameter("@value", SqliteType.Real));

        foreach (var dataRecord in samples)
        {
            upsertCommand.Parameters["@timestamp"].Value = dataRecord.Timestamp;
            upsertCommand.Parameters["@measId"].Value = dataRecord.MeasId;
            upsertCommand.Parameters["@value"].Value = dataRecord.Value;
            upsertCommand.ExecuteNonQuery();
        }
        _ = await Task.FromResult(0);
    }
}
