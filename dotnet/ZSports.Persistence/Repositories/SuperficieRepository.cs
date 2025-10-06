using Microsoft.EntityFrameworkCore;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Superficies;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Repositories;

public class SuperficieRepository(ZSportsDbContext dbContext): ISuperficieRepository
{
    public async Task<SuperficieDto?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Superficie>()
            .Select(s => new SuperficieDto
            {
                Id = s.Id,
                Nombre = s.Nombre
            })
            .FirstOrDefaultAsync(s => s.Nombre == nombre, cancellationToken);
    }
}
