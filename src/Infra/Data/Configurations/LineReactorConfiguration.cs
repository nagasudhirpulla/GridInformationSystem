using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;

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