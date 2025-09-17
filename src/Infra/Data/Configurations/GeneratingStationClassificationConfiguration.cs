using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class GeneratingStationClassificationConfiguration : IEntityTypeConfiguration<GeneratingStationClassification>
{
    public void Configure(EntityTypeBuilder<GeneratingStationClassification> builder)
    {
        builder.HasIndex(u => u.Classification).IsUnique();
    }
}
