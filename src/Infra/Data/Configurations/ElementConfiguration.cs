using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;

namespace Infra.Data.Configurations;

public class ElementConfiguration : IEntityTypeConfiguration<Element>
{
    public void Configure(EntityTypeBuilder<Element> builder)
    {
        builder.HasDiscriminator<string>(nameof(Element.Discriminator));
        builder.HasIndex(e => e.ElementNameCache).IsUnique();

        builder
            .HasOne(o => o.Substation1)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(o => o.Substation2)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
