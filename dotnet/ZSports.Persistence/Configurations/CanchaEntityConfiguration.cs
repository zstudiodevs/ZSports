using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class CanchaEntityConfiguration : IEntityTypeConfiguration<Cancha>
{
    public void Configure(EntityTypeBuilder<Cancha> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Numero)
            .IsRequired();

        builder.Property(c => c.EsIndoor)
            .IsRequired();

        builder.Property(c => c.CapacidadJugadores)
            .IsRequired();

        builder.Property(c => c.DuracionPartido)
            .IsRequired();

        builder.Property(c => c.Activa)
            .IsRequired();

        builder.HasOne(c => c.Superficie)
            .WithMany()
            .HasForeignKey(c => c.SuperficieId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(c => c.Deporte)
            .WithMany()
            .HasForeignKey(c => c.DeporteId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(c => c.Establecimiento)
            .WithMany()
            .HasForeignKey(c => c.EstablecimientoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
