using App.Common.Interfaces;
using Core.Entities;
using Core.Entities.Common;
using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infra.Data.Interceptors;

public class NameCacheUpdatesEventGenerationInterceptor(
    IUser user,
    TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AttachCacheUpdateEventsToEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        AttachCacheUpdateEventsToEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void AttachCacheUpdateEventsToEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State is EntityState.Modified)
            {
                // check the entity being updated and attach relavent domain events for the update
                // so that the events can be dispatched for cache updates before save

                var changedProperties = entry.Properties
                                            .Where(p => p.IsModified)
                                            .Select(p => p.Metadata.Name)
                                            .ToList();
                if (entry.Entity is VoltageLevel v)
                {
                    if (changedProperties.Contains(nameof(VoltageLevel.Level)))
                    {
                        v.AddDomainEvent(new Core.Events.VoltageLevels.VoltageLevelNameChangedEvent(v));
                    }
                }
                if (entry.Entity is Location l)
                {
                    if (changedProperties.Contains(nameof(Location.Name)))
                    {
                        l.AddDomainEvent(new Core.Events.Locations.LocationNameChangedEvent(l));
                    }
                    if (changedProperties.Contains(nameof(Location.StateId)))
                    {
                        l.AddDomainEvent(new Core.Events.Locations.LocationStateChangedEvent(l));
                    }
                }
                if (entry.Entity is Element e)
                {
                    if (changedProperties.Contains(nameof(Element.Name)))
                    {
                        e.AddDomainEvent(new Core.Events.Elements.ElementNameChangedEvent(e));
                    }
                }
                if (entry.Entity is Substation sb)
                {
                    if (changedProperties.Contains(nameof(Substation.Name)))
                    {
                        sb.AddDomainEvent(new Core.Events.Substations.SubstationNameChangedEvent(sb));
                    }
                }
                if (entry.Entity is Line ln)
                {
                    if (changedProperties.Contains(nameof(Line.Name)))
                    {
                        ln.AddDomainEvent(new Core.Events.Lines.LineNameChangedEvent(ln));
                    }
                }
                if (entry.Entity is FilterBank fb)
                {
                    if (changedProperties.Contains(nameof(FilterBank.ElementNumber)))
                    {
                        fb.AddDomainEvent(new Core.Events.FilterBanks.FilterBankNumberChangedEvent(fb));
                    }
                }
            }
        }
    }
}
