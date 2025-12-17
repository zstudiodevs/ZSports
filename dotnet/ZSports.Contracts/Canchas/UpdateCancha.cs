namespace ZSports.Contracts.Canchas;

public record UpdateCancha
{
    public Guid Id { get; init; }
    public int Numero { get; init; }
    public bool EsIndoor { get; init; }
    public int CapacidadJugadores { get; init; }
    public int DuracionPartido { get; init; }
    public Guid SuperficieId { get; init; }
    public Guid DeporteId { get; init; }
    public Guid EstablecimientoId { get; init; }
}
