using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class SubFilterBankConfiguration : IEntityTypeConfiguration<SubFilterBank>
{
    public void Configure(EntityTypeBuilder<SubFilterBank> builder)
    {
        builder
            .HasOne(o => o.FilterBank)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
