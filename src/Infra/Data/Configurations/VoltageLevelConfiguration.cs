using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Configurations;

public class VoltageLevelConfiguration : IEntityTypeConfiguration<VoltageLevel>
{
    public void Configure(EntityTypeBuilder<VoltageLevel> builder)
    {
        builder.HasIndex(u => u.Level).IsUnique();
    }
}
