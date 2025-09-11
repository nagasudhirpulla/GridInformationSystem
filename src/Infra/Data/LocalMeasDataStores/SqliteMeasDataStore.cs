using App.MeasurementData.Interfaces;
using App.Utils;
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

    public List<(int timestamp, float value)> FetchSamples(int measId, DateTime startTime, DateTime endTime)
    {
        List<(int timestamp, float value)> samples = new();
        using (var db = new SqliteConnection(DbConnStr))
        {
            db.Open();
            var selectCommand = new SqliteCommand($@"SELECT {TimeColName},{ValColName} from {MeasDataTableName} 
                                                    where ({MeasIdColName}=@measId) 
                                                    and ({TimeColName} BETWEEN @startTime and @endTime);", db);
            selectCommand.Parameters.AddWithValue("@measId", measId);
            selectCommand.Parameters.AddWithValue("@startTime", TimeUtils.GetUtcMs(startTime));
            selectCommand.Parameters.AddWithValue("@endTime", TimeUtils.GetUtcMs(endTime));

            using var reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                int dt = reader.GetInt32(reader.GetOrdinal(TimeColName));
                float val = reader.GetFloat(reader.GetOrdinal(ValColName));
                samples.Add((dt, val));
            }
        }
        return samples;
    }

    public void InsertSamples(List<(int measId, int timestamp, float value)> samples)
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

        foreach (var s in samples)
        {
            upsertCommand.Parameters["@timestamp"].Value = s.timestamp;
            upsertCommand.Parameters["@measId"].Value = s.measId;
            upsertCommand.Parameters["@value"].Value = s.value;
            upsertCommand.ExecuteNonQuery();
        }
    }
}
