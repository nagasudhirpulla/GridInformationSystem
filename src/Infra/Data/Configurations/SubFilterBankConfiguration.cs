using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;

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
