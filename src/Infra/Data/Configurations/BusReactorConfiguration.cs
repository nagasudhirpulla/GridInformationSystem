using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class BusReactorConfiguration : IEntityTypeConfiguration<BusReactor>
{
    public void Configure(EntityTypeBuilder<BusReactor> builder)
    {
        builder
            .HasOne(o => o.Bus)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
