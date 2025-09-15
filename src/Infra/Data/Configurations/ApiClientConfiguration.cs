using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Data;

namespace Infra.Data.Configurations;

public class ApiClientConfiguration : IEntityTypeConfiguration<ApiClient>
{
    public void Configure(EntityTypeBuilder<ApiClient> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();
        builder.HasIndex(u => u.Key).IsUnique();
    }
}
