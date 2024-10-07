using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;
using Core.Enums;

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
