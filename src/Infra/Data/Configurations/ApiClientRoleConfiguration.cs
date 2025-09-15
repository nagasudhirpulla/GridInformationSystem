using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Data;

namespace Infra.Data.Configurations;

public class ApiClientRoleConfiguration : IEntityTypeConfiguration<ApiClientRole>
{
    public void Configure(EntityTypeBuilder<ApiClientRole> builder)
    {
        builder.HasKey(e => new { e.ApiClientId, e.ApiRoleId });
    }
}
