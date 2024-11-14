using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Configurations;

public class GeneratingStationTypeClassificationConfiguration : IEntityTypeConfiguration<GeneratingStationType>
{
    public void Configure(EntityTypeBuilder<GeneratingStationType> builder)
    {
        builder.HasIndex(u => u.StationType).IsUnique();
    }
}
