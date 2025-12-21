using ZSports.Contracts.Canchas;
using ZSports.Contracts.Usuarios;

namespace ZSports.Contracts.Turnos;

public record TurnoDto
{
    public Guid Id { get; init; }
    public DateOnly Fecha { get; init; }
    public TimeSpan HoraInicio { get; init; }
    public TimeSpan HoraFin { get; init; }
    public string Estado { get; init; } = string.Empty;
    public DateTime FechaCreacion { get; init; }
    public DateTime? FechaConfirmacion { get; init; }
    public DateTime? FechaCancelacion { get; init; }
    public string? MotivoCancelacion { get; init; }
    public CanchaDto Cancha { get; init; } = null!;
    public UsuarioDto UsuarioCreador { get; init; } = null!;
    public IEnumerable<InvitacionTurnoDto> Invitaciones { get; init; } = [];
}