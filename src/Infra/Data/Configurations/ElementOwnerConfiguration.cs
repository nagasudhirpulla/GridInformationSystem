using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Configurations;

public class ElementOwnerConfiguration : IEntityTypeConfiguration<ElementOwner>
{
    public void Configure(EntityTypeBuilder<ElementOwner> builder)
    {
        builder.HasKey(e => new { e.ElementId, e.OwnerId });
    }
}
