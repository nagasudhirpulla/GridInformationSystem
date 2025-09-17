using Core.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class ApiClientRoleConfiguration : IEntityTypeConfiguration<ApiClientRole>
{
    public void Configure(EntityTypeBuilder<ApiClientRole> builder)
    {
        builder.HasKey(e => new { e.ApiClientId, e.ApiRoleId });
    }
}
