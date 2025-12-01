using ZSports.Contracts.Deportes;
using ZSports.Contracts.Establecimientos;
using ZSports.Contracts.Superficies;

namespace ZSports.Contracts.Canchas;

public record CanchaDto
{
    public Guid Id { get; init; }
    public int Numero { get; init; }
    public bool EsIndoor { get; init; } = true;
    public int CapacidadJugadores { get; init; }
    public int DuracionPartido { get; init; }
    public bool Activa { get; init; } = true;
    public SuperficieDto Superficie { get; init; } = null!;
    public DeporteDto Deporte { get; init; } = null!;
    public EstablecimientoDto Establecimiento { get; init; } = null!;
}
