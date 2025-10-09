﻿using App.Common.Interfaces;
using Core.Entities;
using Core.Entities.Data;
using Core.Entities.Elements;
using Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Infra.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
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

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<ElementOwner> ElementOwners => Set<ElementOwner>();

    public DbSet<GridEntityTag> GridEntityTags => Set<GridEntityTag>();

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

    //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    //{
    //    if (_currentTransaction == null)
    //    {
    //        using var transaction = await BeginTransactionAsync(IsolationLevel.Serializable);
    //        try
    //        {
    //            var result = await base.SaveChangesAsync(cancellationToken);
    //            await DispatchEvents();
    //            await CommitTransactionAsync(transaction);
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            RollbackTransaction();
    //            Console.WriteLine($"rolling back changes occured while saving DB changes, {ex.Message}");
    //            throw;
    //        }
    //    }
    //    else
    //    {
    //        var result = await base.SaveChangesAsync(cancellationToken);
    //        await DispatchEvents();
    //        return result;
    //    }
    //}

    //private async Task DispatchEvents()
    //{
    //    var entities = ChangeTracker
    //        .Entries<BaseEntity>()
    //        .Where(e => e.Entity.DomainEvents.Count != 0)
    //        .Select(e => e.Entity);

    //    var domainEvents = entities
    //        .SelectMany(e => e.DomainEvents)
    //        .ToList();

    //    entities.ToList().ForEach(e => e.ClearDomainEvents());

    //    foreach (var domainEvent in domainEvents)
    //        await mediator.Publish(domainEvent);
    //}
}
