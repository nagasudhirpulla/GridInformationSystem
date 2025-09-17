using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class LineReactorConfiguration : IEntityTypeConfiguration<LineReactor>
{
    public void Configure(EntityTypeBuilder<LineReactor> builder)
    {
        builder
            .HasOne(o => o.Line)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}