using ZSports.Contracts.Superficies;

namespace ZSports.Contracts.Services;

public interface ISuperficieService
{
    Task<IEnumerable<SuperficieDto>> GetAllAsync(bool includeDisabled = false, CancellationToken cancellationToken = default);
    Task<SuperficieDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SuperficieDto> CreateAsync(string superficie, CancellationToken cancellationToken = default);
    Task<SuperficieDto> UpdateAsync(SuperficieDto superficieDto, CancellationToken cancellationToken = default);
}
