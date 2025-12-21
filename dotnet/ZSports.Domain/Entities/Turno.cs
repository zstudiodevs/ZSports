using ZSports.Domain.Enums;

namespace ZSports.Domain.Entities;

public class Turno
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateOnly Fecha { get; private set; }
    public TimeSpan HoraInicio { get; private set; }
    public TimeSpan HoraFin { get; private set; }
    public EstadoTurno Estado { get; private set; } = EstadoTurno.Pendiente;
    public DateTime FechaCreacion { get; private set; } = DateTime.UtcNow;
    public DateTime? FechaConfirmacion { get; private set; }
    public DateTime? FechaCancelacion { get; private set; }
    public string? MotivoCancelacion { get; private set; }

    // Relaciones
    public Guid CanchaId { get; private set; }
    public virtual Cancha Cancha { get; private set; } = null!;

    public Guid UsuarioCreadorId { get; private set; }
    public virtual Usuario UsuarioCreador { get; private set; } = null!;

    public virtual ICollection<InvitacionTurno> Invitaciones { get; private set; } = [];

    public void SetFecha(DateOnly fecha)
    {
        if (fecha < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new ArgumentException("No se pueden crear turnos en fechas pasadas.");
        }

        Fecha = fecha;
    }

    public void SetHorarios(TimeSpan horaInicio, int duracionMinutos, TimeSpan horaInicioMinimaEstablecimiento, TimeSpan horaFinMaximaEstablecimiento)
    {
        if (duracionMinutos <= 0)
        {
            throw new ArgumentException("La duración debe ser mayor a cero.");
        }

        var horaFin = horaInicio.Add(TimeSpan.FromMinutes(duracionMinutos));

        // Validar que esté dentro del horario del establecimiento
        if (horaInicio < horaInicioMinimaEstablecimiento)
        {
            throw new ArgumentException($"El turno no puede iniciar antes de las {horaInicioMinimaEstablecimiento:hh\\:mm}.");
        }

        if (horaFin > horaFinMaximaEstablecimiento)
        {
            throw new ArgumentException($"El turno no puede finalizar después de las {horaFinMaximaEstablecimiento:hh\\:mm}.");
        }

        HoraInicio = horaInicio;
        HoraFin = horaFin;
    }

    public void SetCancha(Cancha cancha)
    {
        Cancha = cancha ?? throw new ArgumentNullException(nameof(cancha), "La cancha no puede ser nula.");
        CanchaId = cancha.Id;
    }

    public void SetUsuarioCreador(Usuario usuario)
    {
        UsuarioCreador = usuario ?? throw new ArgumentNullException(nameof(usuario), "El usuario creador no puede ser nulo.");
        UsuarioCreadorId = usuario.Id;
    }

    public void Confirmar()
    {
        if (Estado == EstadoTurno.Cancelado)
        {
            throw new InvalidOperationException("No se puede confirmar un turno cancelado.");
        }

        if (Estado == EstadoTurno.Completado)
        {
            throw new InvalidOperationException("No se puede confirmar un turno completado.");
        }

        Estado = EstadoTurno.Confirmado;
        FechaConfirmacion = DateTime.UtcNow;
    }

    public void Cancelar(string? motivo = null)
    {
        if (Estado == EstadoTurno.Completado)
        {
            throw new InvalidOperationException("No se puede cancelar un turno completado.");
        }

        if (Estado == EstadoTurno.Cancelado)
        {
            throw new InvalidOperationException("El turno ya está cancelado.");
        }

        // Validar que falte más de 1 hora para el inicio
        var timeOnlyInicio = TimeOnly.FromTimeSpan(HoraInicio);
        var fechaHoraInicio = Fecha.ToDateTime(timeOnlyInicio);
        var ahora = DateTime.Now;

        if (fechaHoraInicio.AddHours(-1) <= ahora)
        {
            throw new InvalidOperationException("No se puede cancelar el turno con menos de 1 hora de anticipación.");
        }

        Estado = EstadoTurno.Cancelado;
        FechaCancelacion = DateTime.UtcNow;
        MotivoCancelacion = motivo;
    }

    public void Completar()
    {
        if (Estado == EstadoTurno.Cancelado)
        {
            throw new InvalidOperationException("No se puede completar un turno cancelado.");
        }

        Estado = EstadoTurno.Completado;
    }

    public void ActualizarHorarios(TimeSpan nuevaHoraInicio, int duracionMinutos, TimeSpan horaInicioMinimaEstablecimiento, TimeSpan horaFinMaximaEstablecimiento)
    {
        if (Estado == EstadoTurno.Confirmado)
        {
            throw new InvalidOperationException("No se puede modificar un turno confirmado.");
        }

        if (Estado == EstadoTurno.Cancelado)
        {
            throw new InvalidOperationException("No se puede modificar un turno cancelado.");
        }

        if (Estado == EstadoTurno.Completado)
        {
            throw new InvalidOperationException("No se puede modificar un turno completado.");
        }

        SetHorarios(nuevaHoraInicio, duracionMinutos, horaInicioMinimaEstablecimiento, horaFinMaximaEstablecimiento);
    }

    public int ObtenerCantidadInvitacionesAceptadas()
    {
        return Invitaciones.Count(i => i.Estado == EstadoInvitacion.Aceptada);
    }

    public bool DebeConfirmarseAutomaticamente(int capacidadCancha)
    {
        // +1 por el usuario creador
        var totalJugadores = ObtenerCantidadInvitacionesAceptadas() + 1;
        return totalJugadores >= capacidadCancha;
    }
}