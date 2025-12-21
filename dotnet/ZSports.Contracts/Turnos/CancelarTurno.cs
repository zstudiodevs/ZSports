namespace ZSports.Contracts.Turnos;

public record CancelarTurno
{
    public Guid TurnoId { get; init; }
    public Guid UsuarioId { get; init; } // Para validar que es el creador
    public string? Motivo { get; init; }
}