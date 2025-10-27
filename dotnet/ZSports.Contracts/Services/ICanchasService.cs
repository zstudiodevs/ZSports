using ZSports.Contracts.Canchas;

namespace ZSports.Contracts.Services;

public interface ICanchasService
{
    Task<CanchaDto> CreateCanchaAsync(CreateCancha crearCancha, CancellationToken cancellationToken = default);
    Task<CanchaDto> GetCanchaByIdAsync(Guid canchaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CanchaDto>> GetAllCanchasAsync(CancellationToken cancellationToken = default);
}
