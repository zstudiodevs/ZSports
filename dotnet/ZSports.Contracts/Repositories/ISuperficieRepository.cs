using ZSports.Domain.Entities;

namespace ZSports.Contracts.Repositories;

public interface ISuperficieRepository
{
    Task<Superficie?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);
}
