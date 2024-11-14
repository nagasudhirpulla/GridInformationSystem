using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Configurations;

public class GeneratingStationClassificationConfiguration : IEntityTypeConfiguration<GeneratingStationClassification>
{
    public void Configure(EntityTypeBuilder<GeneratingStationClassification> builder)
    {
        builder.HasIndex(u => u.Classification).IsUnique();        
    }
}
