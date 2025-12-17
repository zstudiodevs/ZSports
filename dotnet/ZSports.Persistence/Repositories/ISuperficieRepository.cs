using ZSports.Contracts.Superficies;

namespace ZSports.Persistence.Repositories;

public interface ISuperficieRepository
{
    Task<SuperficieDto?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);
}
