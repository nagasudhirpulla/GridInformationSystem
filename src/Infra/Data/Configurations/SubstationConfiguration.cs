using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Configurations;

public class SubstationConfiguration : IEntityTypeConfiguration<Substation>
{
    public void Configure(EntityTypeBuilder<Substation> builder)
    {
        builder.HasIndex(u => u.NameCache).IsUnique();

        builder
            .HasOne(o => o.VoltageLevel)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(o => o.Location)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
