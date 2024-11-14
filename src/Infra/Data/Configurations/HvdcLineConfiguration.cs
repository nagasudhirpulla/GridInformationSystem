using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;

namespace Infra.Data.Configurations;

public class HvdcLineConfiguration : IEntityTypeConfiguration<HvdcLine>
{
    public void Configure(EntityTypeBuilder<HvdcLine> builder)
    {
        builder
            .HasOne(o => o.Bus1)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(o => o.Bus2)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
