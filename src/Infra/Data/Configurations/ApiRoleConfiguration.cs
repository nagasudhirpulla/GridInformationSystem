using Core.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class ApiRoleConfiguration : IEntityTypeConfiguration<ApiRole>
{
    public void Configure(EntityTypeBuilder<ApiRole> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();
    }
}
