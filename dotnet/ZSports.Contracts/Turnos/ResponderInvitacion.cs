namespace ZSports.Contracts.Turnos;

public record ResponderInvitacion
{
    public Guid InvitacionId { get; init; }
    public Guid UsuarioInvitadoId { get; init; } // Para validar que es el invitado
    public bool Aceptar { get; init; }
}