using App.Common.Interfaces;
using Core.Entities;
using Core.Entities.Common;
using Core.Entities.Elements;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
