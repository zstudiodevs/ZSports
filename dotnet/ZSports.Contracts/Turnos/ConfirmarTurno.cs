namespace ZSports.Contracts.Turnos;

public record ConfirmarTurno
{
    public Guid TurnoId { get; init; }
    public Guid UsuarioId { get; init; } // Para validar que es el creador
}