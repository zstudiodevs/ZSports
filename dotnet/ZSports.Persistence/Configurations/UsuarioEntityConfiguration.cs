using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Apellido)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.FechaAlta)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UltimoAcceso)
            .IsRequired(false);

        builder.Property(u => u.Activo)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
