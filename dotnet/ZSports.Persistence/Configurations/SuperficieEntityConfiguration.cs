using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class SuperficieEntityConfiguration : IEntityTypeConfiguration<Superficie>
{
    public void Configure(EntityTypeBuilder<Superficie> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Nombre)
            .HasMaxLength(64)
            .IsRequired()
            .UseCollation("Latin1_General_100_CI_AI_SC");

        builder.Property(x => x.Activo)
            .IsRequired();

        builder.HasIndex(x => x.Nombre)
        .IsUnique()
        .HasDatabaseName("UQ_Superficie_Nombre");
    }
}
