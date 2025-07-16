using Core.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class DataSourceConfiguration : IEntityTypeConfiguration<Datasource>
{
    public void Configure(EntityTypeBuilder<Datasource> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();
    }
}
