namespace ZSports.Contracts.Turnos;

public record UpdateTurno
{
    public Guid Id { get; init; }
    public DateOnly Fecha { get; init; }
    public TimeSpan HoraInicio { get; init; }
    public Guid CanchaId { get; init; }
    public Guid UsuarioCreadorId { get; init; }
}