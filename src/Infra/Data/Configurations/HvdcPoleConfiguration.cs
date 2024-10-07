using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;
using Core.Enums;

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