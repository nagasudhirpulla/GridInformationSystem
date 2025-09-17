using Core.Entities.Elements;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class TransformerConfiguration : IEntityTypeConfiguration<Transformer>
{
    public void Configure(EntityTypeBuilder<Transformer> builder)
    {
        builder.Property(p => p.TransformerType)
            .HasConversion(
                p => p.Value,
                p => TransformerTypeEnum.FromValue(p));

    }
}