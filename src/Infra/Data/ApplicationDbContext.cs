using App.Common.Interfaces;
using Core.Entities;
using Core.Entities.Data;
using Core.Entities.Elements;
using Infra.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Infra.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private IDbContextTransaction? _currentTransaction;


    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<IDbContextTransaction?> BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(isolationLevel);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction? transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task ReplaceSubstringInColumn(string oldValue, string newValue, string colName, CancellationToken cancellationToken)
    {
        await ReplaceColumnSubstring.ExecuteAsync(this, oldValue, newValue, colName, cancellationToken);
    }
}
