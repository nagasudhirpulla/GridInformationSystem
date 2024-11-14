using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Configurations;

public class GeneratingStationConfiguration : IEntityTypeConfiguration<GeneratingStation>
{
    public void Configure(EntityTypeBuilder<GeneratingStation> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();

        builder
            .HasOne(o => o.Fuel)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(o => o.GeneratingStationType)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(o => o.GeneratingStationClassification)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
