using Core.Entities.Elements;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class BusConfiguration : IEntityTypeConfiguration<Bus>
{
    public void Configure(EntityTypeBuilder<Bus> builder)
    {
        builder.Property(p => p.BusType)
            .HasConversion(
                p => p.Value,
                p => BusTypeEnum.FromValue(p));

    }
}
