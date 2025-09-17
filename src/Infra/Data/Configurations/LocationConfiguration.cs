using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();

        builder
            .HasOne(o => o.State)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
