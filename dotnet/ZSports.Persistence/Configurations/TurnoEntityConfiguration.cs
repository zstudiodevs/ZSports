using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class TurnoEntityConfiguration : IEntityTypeConfiguration<Turno>
{
    public void Configure(EntityTypeBuilder<Turno> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Fecha)
            .IsRequired();

        builder.Property(t => t.HoraInicio)
            .IsRequired();

        builder.Property(t => t.HoraFin)
            .IsRequired();

        builder.Property(t => t.Estado)
            .IsRequired()
            .HasConversion<int>(); // Removido HasDefaultValue

        builder.Property(t => t.FechaCreacion)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.FechaConfirmacion)
            .IsRequired(false);

        builder.Property(t => t.FechaCancelacion)
            .IsRequired(false);

        builder.Property(t => t.MotivoCancelacion)
            .HasMaxLength(500);

        // Relación con Cancha
        builder.HasOne(t => t.Cancha)
            .WithMany()
            .HasForeignKey(t => t.CanchaId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Relación con Usuario Creador
        builder.HasOne(t => t.UsuarioCreador)
            .WithMany()
            .HasForeignKey(t => t.UsuarioCreadorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Relación con Invitaciones
        builder.HasMany(t => t.Invitaciones)
            .WithOne(i => i.Turno)
            .HasForeignKey(i => i.TurnoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice para búsquedas por cancha y fecha
        builder.HasIndex(t => new { t.CanchaId, t.Fecha, t.Estado })
            .HasDatabaseName("IX_Turno_Cancha_Fecha_Estado");

        // Índice para búsquedas por usuario
        builder.HasIndex(t => t.UsuarioCreadorId)
            .HasDatabaseName("IX_Turno_UsuarioCreador");
    }
}