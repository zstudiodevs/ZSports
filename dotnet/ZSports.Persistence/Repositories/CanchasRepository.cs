using Microsoft.EntityFrameworkCore;
using ZSports.Contracts.Repositories;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Repositories;

public class CanchasRepository(ZSportsDbContext dbContext) : ICanchasRepository
{
    public async Task<bool> ExisteNumeroCanchaEnEstablecimientoAsync(
        int numero,
        Guid establecimientoId,
        Guid? excludeCanchaId = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Cancha>()
            .Where(c => c.Numero == numero && c.EstablecimientoId == establecimientoId);

        if (excludeCanchaId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCanchaId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Cancha> Items, int TotalCount)> GetCanchasFiltradas(
        Guid establecimientoId,
        Guid? deporteId,
        bool? soloActivas,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Cancha>()
            .Include(c => c.Superficie)
            .Include(c => c.Deporte)
            .Include(c => c.Establecimiento)
                .ThenInclude(e => e.Propietario)
            .Where(c => c.EstablecimientoId == establecimientoId);

        if (deporteId.HasValue)
        {
            query = query.Where(c => c.DeporteId == deporteId.Value);
        }

        if (soloActivas.HasValue )
        {
            query = query.Where(c => c.Activa == soloActivas.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Numero)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}