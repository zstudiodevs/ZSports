using ZSports.Contracts.Canchas;
using ZSports.Contracts.Common;

namespace ZSports.Contracts.Services;

public interface ICanchasService
{
    Task<CanchaDto> CrearCanchaAsync(CreateCancha request, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<CanchaDto>> ObtenerCanchasAsync(ObtenerCanchasRequest request, CancellationToken cancellationToken = default);
    Task<CanchaDto> ObtenerCanchaPorIdAsync(Guid canchaId, CancellationToken cancellationToken = default);
    Task<CanchaDto> ActualizarCanchaAsync(UpdateCancha request, CancellationToken cancellationToken = default);
    Task<bool> CambiarEstadoCanchaAsync(CambiarEstadoCancha request, CancellationToken cancellationToken = default);
    Task<IEnumerable<TurnoDisponibleDto>> ObtenerTurnosDisponiblesAsync(ObtenerTurnosDisponiblesRequest request, CancellationToken cancellationToken = default);
}