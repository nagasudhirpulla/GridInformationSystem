using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class GeneratingStationTypeClassificationConfiguration : IEntityTypeConfiguration<GeneratingStationType>
{
    public void Configure(EntityTypeBuilder<GeneratingStationType> builder)
    {
        builder.HasIndex(u => u.StationType).IsUnique();
    }
}
