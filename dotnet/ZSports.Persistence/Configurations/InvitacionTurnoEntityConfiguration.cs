using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class InvitacionTurnoEntityConfiguration : IEntityTypeConfiguration<InvitacionTurno>
{
    public void Configure(EntityTypeBuilder<InvitacionTurno> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Estado)
            .IsRequired()
            .HasConversion<int>(); // Removido HasDefaultValue

        builder.Property(i => i.FechaInvitacion)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(i => i.FechaRespuesta)
            .IsRequired(false);

        // Relación con Turno
        builder.HasOne(i => i.Turno)
            .WithMany(t => t.Invitaciones)
            .HasForeignKey(i => i.TurnoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Relación con Usuario Invitado
        builder.HasOne(i => i.UsuarioInvitado)
            .WithMany()
            .HasForeignKey(i => i.UsuarioInvitadoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Índice único: un usuario no puede ser invitado dos veces al mismo turno
        builder.HasIndex(i => new { i.TurnoId, i.UsuarioInvitadoId })
            .IsUnique()
            .HasDatabaseName("UQ_InvitacionTurno_Turno_Usuario");

        // Índice para búsquedas por usuario invitado
        builder.HasIndex(i => i.UsuarioInvitadoId)
            .HasDatabaseName("IX_InvitacionTurno_UsuarioInvitado");
    }
}