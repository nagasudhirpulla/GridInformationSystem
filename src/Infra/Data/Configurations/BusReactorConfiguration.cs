using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;

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
