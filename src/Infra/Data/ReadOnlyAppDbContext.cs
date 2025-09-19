using App.Common.Interfaces;
using Core.Entities;
using Core.Entities.Data;
using Core.Entities.Elements;
using Infra.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infra.Data;

public class ReadOnlyAppDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<Fuel> Fuels => Set<Fuel>();

    public DbSet<GeneratingStation> GeneratingStations => Set<GeneratingStation>();

    public DbSet<GeneratingStationClassification> GeneratingStationClassifications => Set<GeneratingStationClassification>();

    public DbSet<GeneratingStationType> GeneratingStationTypes => Set<GeneratingStationType>();

    public DbSet<Location> Locations => Set<Location>();

    public DbSet<Owner> Owners => Set<Owner>();

    public DbSet<Region> Regions => Set<Region>();

    public DbSet<State> States => Set<State>();

    public DbSet<Substation> Substations => Set<Substation>();

    public DbSet<VoltageLevel> VoltageLevels => Set<VoltageLevel>();

    public DbSet<ElementOwner> ElementOwners => Set<ElementOwner>();

    public DbSet<SubstationOwner> SubstationOwners => Set<SubstationOwner>();

    public DbSet<GridEntity> GridEntities => Set<GridEntity>();
    public DbSet<Element> Elements => Set<Element>();
    public DbSet<Bay> Bays => Set<Bay>();

    public DbSet<Bus> Buses => Set<Bus>();

    public DbSet<BusReactor> BusReactors => Set<BusReactor>();

    public DbSet<FilterBank> FilterBanks => Set<FilterBank>();

    public DbSet<GeneratingUnit> GeneratingUnits => Set<GeneratingUnit>();

    public DbSet<HvdcLine> HvdcLines => Set<HvdcLine>();

    public DbSet<HvdcPole> HvdcPoles => Set<HvdcPole>();

    public DbSet<Line> Lines => Set<Line>();

    public DbSet<LineReactor> LineReactors => Set<LineReactor>();

    public DbSet<SubFilterBank> SubFilterBanks => Set<SubFilterBank>();

    public DbSet<Transformer> Transformers => Set<Transformer>();


    public DbSet<Metric> Metrics => Set<Metric>();
    public DbSet<Measurement> Measurements => Set<Measurement>();
    public DbSet<Datasource> Datasources => Set<Datasource>();
    public DbSet<ProxyDatasource> ProxyDatasources => Set<ProxyDatasource>();
    public DbSet<ApiClient> ApiClients => Set<ApiClient>();
    public DbSet<ApiRole> ApiRoles => Set<ApiRole>();
    public DbSet<ApiClientRole> ApiClientRoles => Set<ApiClientRole>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        base.OnConfiguring(optionsBuilder);
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    private readonly IDbContextTransaction? _currentTransaction;


    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => false;

    public async Task CommitTransactionAsync(IDbContextTransaction? transaction)
    {
        _ = await Task.FromResult(0);
        throw new NotImplementedException();
    }

    public void RollbackTransaction()
    {
        throw new NotImplementedException();
    }

    public Task<IDbContextTransaction?> BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceSubstringInColumn(string oldValue, string newValue, string colName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}