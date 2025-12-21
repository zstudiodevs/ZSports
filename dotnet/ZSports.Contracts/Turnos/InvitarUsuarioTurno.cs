namespace ZSports.Contracts.Turnos;

public record InvitarUsuarioTurno
{
    public Guid TurnoId { get; init; }
    public Guid UsuarioInvitadoId { get; init; }
    public Guid UsuarioCreadorId { get; init; } // Para validar que es el creador
}