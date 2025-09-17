using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class LineConfiguration : IEntityTypeConfiguration<Line>
{
    public void Configure(EntityTypeBuilder<Line> builder)
    {
        builder
            .HasOne(o => o.Bus1)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(o => o.Bus2)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}