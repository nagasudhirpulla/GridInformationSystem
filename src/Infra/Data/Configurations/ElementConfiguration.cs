using Core.Entities.Elements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class ElementConfiguration : IEntityTypeConfiguration<Element>
{
    public void Configure(EntityTypeBuilder<Element> builder)
    {
        builder.HasDiscriminator<string>(nameof(Element.Discriminator));
        builder.HasIndex(e => e.Name).IsUnique();

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
