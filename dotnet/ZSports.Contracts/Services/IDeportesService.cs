using ZSports.Contracts.Deportes;

namespace ZSports.Contracts.Services;

public interface IDeportesService
{
    Task<DeporteDto> CreateDeporteAsync(CreateDeporte createDeporte, CancellationToken cancellationToken = default);
    Task<DeporteDto> GetDeporteByIdAsync(Guid deporteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeporteDto>> GetAllDeportesAsync(CancellationToken cancellationToken = default);
}
