namespace ZSports.Contracts.Canchas;

public record TurnoDisponibleDto
{
    public TimeSpan HoraInicio { get; init; }
    public TimeSpan HoraFin { get; init; }
    public bool Disponible { get; init; }
}