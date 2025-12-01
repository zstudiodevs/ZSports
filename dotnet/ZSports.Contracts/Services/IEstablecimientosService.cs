using ZSports.Contracts.Establecimientos;

namespace ZSports.Contracts.Services;

public interface IEstablecimientosService
{
    Task<EstablecimientoDto> CreateEstablecimientoAsync(CrearEstablecimiento crearEstablecimiento, CancellationToken cancellationToken = default);
    Task<EstablecimientoDto> GetEstablecimientoByIdAsync(Guid establecimientoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EstablecimientoDto>> GetAllEstablecimientosAsync(CancellationToken cancellationToken = default);
    Task<EstablecimientoDto> GetEstablecimientoByPropietarioId(Guid propietarioId, CancellationToken cancellationToken = default);
}
