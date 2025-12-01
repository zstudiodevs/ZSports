namespace ZSports.Contracts.Canchas;

public record CreateCancha
{
    public int Numero { get; init; }
    public bool EsIndoor { get; init; } = true;
    public int CapacidadJugadores { get; init; }
    public int DuracionPartido { get; init; }
    public bool Activa { get; init; } = true;
    public Guid SuperficieId { get; init; }
    public Guid DeporteId { get; init; }
    public Guid EstablecimientoId { get; init; }
}
