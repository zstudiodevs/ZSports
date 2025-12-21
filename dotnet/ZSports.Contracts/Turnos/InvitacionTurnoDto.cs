using ZSports.Contracts.Usuarios;

namespace ZSports.Contracts.Turnos;

public record InvitacionTurnoDto
{
    public Guid Id { get; init; }
    public string Estado { get; init; } = string.Empty;
    public DateTime FechaInvitacion { get; init; }
    public DateTime? FechaRespuesta { get; init; }
    public UsuarioDto UsuarioInvitado { get; init; } = null!;
}