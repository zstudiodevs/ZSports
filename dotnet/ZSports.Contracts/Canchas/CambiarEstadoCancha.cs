namespace ZSports.Contracts.Canchas;

public record CambiarEstadoCancha
{
    public Guid Id { get; init; }
    public bool Activa { get; init; }
}
