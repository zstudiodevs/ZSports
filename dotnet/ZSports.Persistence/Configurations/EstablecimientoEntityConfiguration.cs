using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZSports.Domain.Constants;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class EstablecimientoEntityConfiguration : IEntityTypeConfiguration<Establecimiento>
{
    public void Configure(EntityTypeBuilder<Establecimiento> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(EstablecimientosConstants.MaxNombreLength);

        builder.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(EstablecimientosConstants.MaxDescripcionLength);

        builder.Property(e => e.Telefono)
            .IsRequired()
            .HasMaxLength(EstablecimientosConstants.MaxTelefonoLength);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(EstablecimientosConstants.MaxEmailLength);

        builder.Property(e => e.Activo)
            .IsRequired();

        builder.HasOne(e => e.Propietario)
            .WithMany()
            .HasForeignKey(e => e.PropietarioId)
            .IsRequired();
    }
}
