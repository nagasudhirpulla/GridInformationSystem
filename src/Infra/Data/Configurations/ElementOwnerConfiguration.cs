using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class ElementOwnerConfiguration : IEntityTypeConfiguration<ElementOwner>
{
    public void Configure(EntityTypeBuilder<ElementOwner> builder)
    {
        builder.HasKey(e => new { e.ElementId, e.OwnerId });
    }
}
