using ZSports.Domain.Entities;

namespace ZSports.Persistence.Repositories;

public interface ICanchasRepository
{
    Task<bool> ExisteNumeroCanchaEnEstablecimientoAsync(int numero, Guid establecimientoId, Guid? excludeCanchaId = null, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Cancha> Items, int TotalCount)> GetCanchasFiltradas(
        Guid establecimientoId,
        Guid? deporteId,
        bool? soloActivas,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
