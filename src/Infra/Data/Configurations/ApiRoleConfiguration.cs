using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Data;

namespace Infra.Data.Configurations;

public class ApiRoleConfiguration : IEntityTypeConfiguration<ApiRole>
{
    public void Configure(EntityTypeBuilder<ApiRole> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();
    }
}
