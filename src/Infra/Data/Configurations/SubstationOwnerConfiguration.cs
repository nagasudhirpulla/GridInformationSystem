using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class SubstationOwnerConfiguration : IEntityTypeConfiguration<SubstationOwner>
{
    public void Configure(EntityTypeBuilder<SubstationOwner> builder)
    {
        builder.HasKey(e => new { e.SubstationId, e.OwnerId });
    }
}
