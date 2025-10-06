using ZSports.Contracts.Superficies;

namespace ZSports.Contracts.Repositories;

public interface ISuperficieRepository
{
    Task<SuperficieDto?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);
}
