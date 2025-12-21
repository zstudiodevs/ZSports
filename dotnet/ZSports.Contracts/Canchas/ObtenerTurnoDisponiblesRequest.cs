namespace ZSports.Contracts.Canchas;

public record ObtenerTurnosDisponiblesRequest
{
    public Guid CanchaId { get; init; }
    public DateOnly Fecha { get; init; }
}