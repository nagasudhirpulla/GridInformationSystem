using Core.Entities.Common;

namespace Core.Entities.Data;

public class Measurement : AuditableEntity
{
    public GridEntity Entity { get; set; } = null!;
    public int EntityId { get; set; }

    public Metric Metric { get; set; } = null!;
    public int MetricId { get; set; }

    public Datasource Datasource { get; set; } = null!;
    public int DatasourceId { get; set; }

    public required string HistorianPntId { get; set; }
}