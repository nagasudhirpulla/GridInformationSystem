using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Elements;
using Core.Enums;

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