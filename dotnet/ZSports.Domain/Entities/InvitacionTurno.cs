using ZSports.Domain.Enums;

namespace ZSports.Domain.Entities;

public class InvitacionTurno
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public EstadoInvitacion Estado { get; private set; } = EstadoInvitacion.Pendiente;
    public DateTime FechaInvitacion { get; private set; } = DateTime.UtcNow;
    public DateTime? FechaRespuesta { get; private set; }

    // Relaciones
    public Guid TurnoId { get; private set; }
    public virtual Turno Turno { get; private set; } = null!;

    public Guid UsuarioInvitadoId { get; private set; }
    public virtual Usuario UsuarioInvitado { get; private set; } = null!;

    public void SetTurno(Turno turno)
    {
        Turno = turno ?? throw new ArgumentNullException(nameof(turno), "El turno no puede ser nulo.");
        TurnoId = turno.Id;
    }

    public void SetUsuarioInvitado(Usuario usuario)
    {
        UsuarioInvitado = usuario ?? throw new ArgumentNullException(nameof(usuario), "El usuario invitado no puede ser nulo.");
        UsuarioInvitadoId = usuario.Id;
    }

    public void Aceptar()
    {
        if (Estado != EstadoInvitacion.Pendiente)
        {
            throw new InvalidOperationException("Solo se pueden aceptar invitaciones pendientes.");
        }

        Estado = EstadoInvitacion.Aceptada;
        FechaRespuesta = DateTime.UtcNow;
    }

    public void Rechazar()
    {
        if (Estado != EstadoInvitacion.Pendiente)
        {
            throw new InvalidOperationException("Solo se pueden rechazar invitaciones pendientes.");
        }

        Estado = EstadoInvitacion.Rechazada;
        FechaRespuesta = DateTime.UtcNow;
    }
}