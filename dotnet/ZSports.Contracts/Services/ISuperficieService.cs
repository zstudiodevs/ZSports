using ZSports.Contracts.Superficies;

namespace ZSports.Contracts.Services;

public interface ISuperficieService
{
    Task<IEnumerable<SuperficieDto>> GetAllSuperficiesAsync(CancellationToken cancellationToken = default);
    Task<SuperficieDto> CreateAsync(string superficie, CancellationToken cancellationToken = default);

}
