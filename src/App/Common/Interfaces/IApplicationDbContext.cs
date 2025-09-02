using Core.Entities;
using Core.Entities.Data;
using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace App.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<GridEntity> GridEntities { get; }
    DbSet<Fuel> Fuels { get; }
    DbSet<GeneratingStation> GeneratingStations { get; }
    DbSet<GeneratingStationClassification> GeneratingStationClassifications { get; }
    DbSet<GeneratingStationType> GeneratingStationTypes { get; }
    DbSet<Location> Locations { get; }
    DbSet<Owner> Owners { get; }
    DbSet<Region> Regions { get; }
    DbSet<State> States { get; }
    DbSet<Substation> Substations { get; }
    DbSet<VoltageLevel> VoltageLevels { get; }

    // Many to Many tables
    DbSet<ElementOwner> ElementOwners { get; }
    DbSet<SubstationOwner> SubstationOwners { get; }

    // Elements
    DbSet<Element> Elements { get; }
    DbSet<Bay> Bays { get; }
    DbSet<Bus> Buses { get; }
    DbSet<BusReactor> BusReactors { get; }
    DbSet<FilterBank> FilterBanks { get; }
    DbSet<GeneratingUnit> GeneratingUnits { get; }
    DbSet<HvdcLine> HvdcLines { get; }
    DbSet<HvdcPole> HvdcPoles { get; }
    DbSet<Line> Lines { get; }
    DbSet<LineReactor> LineReactors { get; }
    DbSet<SubFilterBank> SubFilterBanks { get; }
    DbSet<Transformer> Transformers { get; }
    DbSet<Datasource> Datasources { get; }
    DbSet<ProxyDatasource> ProxyDatasources { get; }
    DbSet<Metric> Metrics { get; }
    DbSet<Measurement> Measurements { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    IDbContextTransaction? GetCurrentTransaction();
    bool HasActiveTransaction { get; }
    Task<IDbContextTransaction?> BeginTransactionAsync(IsolationLevel isolationLevel);
    Task CommitTransactionAsync(IDbContextTransaction? transaction);
    void RollbackTransaction();
    Task ReplaceSubstringInColumn(string oldValue, string newValue, string colName, CancellationToken cancellationToken);
}
