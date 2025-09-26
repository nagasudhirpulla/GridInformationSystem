using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class GridEntityTagConfiguration : IEntityTypeConfiguration<GridEntityTag>
{
    public void Configure(EntityTypeBuilder<GridEntityTag> builder)
    {
        builder.HasKey(e => new { e.GridEntityId, e.TagId });
    }
}
