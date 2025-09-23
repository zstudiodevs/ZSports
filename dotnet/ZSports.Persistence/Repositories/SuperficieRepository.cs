using Microsoft.EntityFrameworkCore;
using ZSports.Contracts.Repositories;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Repositories;

public class SuperficieRepository(ZSportsDbContext dbContext): ISuperficieRepository
{
    public async Task<Superficie?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Superficie>()
            .FirstOrDefaultAsync(s => s.Nombre == nombre, cancellationToken);
    }
}
