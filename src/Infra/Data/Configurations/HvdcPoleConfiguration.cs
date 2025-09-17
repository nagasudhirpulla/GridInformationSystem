using Core.Entities.Elements;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class HvdcPoleConfiguration : IEntityTypeConfiguration<HvdcPole>
{
    public void Configure(EntityTypeBuilder<HvdcPole> builder)
    {
        builder.Property(p => p.PoleType)
            .HasConversion(
                p => p.Value,
                p => HvdcPoleTypeEnum.FromValue(p));

    }
}