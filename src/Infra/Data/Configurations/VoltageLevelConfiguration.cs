using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class VoltageLevelConfiguration : IEntityTypeConfiguration<VoltageLevel>
{
    public void Configure(EntityTypeBuilder<VoltageLevel> builder)
    {
        builder.HasIndex(u => u.Level).IsUnique();
    }
}
