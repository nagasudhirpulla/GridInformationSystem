using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Configurations;

public class SubstationOwnerConfiguration : IEntityTypeConfiguration<SubstationOwner>
{
    public void Configure(EntityTypeBuilder<SubstationOwner> builder)
    {
        builder.HasKey(e => new { e.SubstationId, e.OwnerId });
    }
}
