using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Contracts.Superficies;
using ZSports.Domain.Entities;

namespace ZSports.Api.Services;

public class SuperficieService(
    ILogger<SuperficieService> logger,
    IRepository<Superficie, Guid> genericRepository,
    ISuperficieRepository superficieRepository) : ISuperficieService
{
    public async Task<SuperficieDto> CreateAsync(string superficie, CancellationToken cancellationToken = default)
    {
        using(var transaction = await genericRepository.BeginTransaction())
        {
            try
            {
                logger.LogInformation("Iniciando el proceso de creacion de superficie: {Superficie}", superficie);

                logger.LogInformation("Verificando si la superficie {Superficie} ya existe", superficie);
                var superficieExistente = await superficieRepository.GetByNombreAsync(superficie, cancellationToken);

                if (superficieExistente is not null)
                {
                    logger.LogInformation("La superficie {Superficie} ya existe con Id: {Id}", superficie, superficieExistente.Id);
                    return new SuperficieDto
                    {
                        Id = superficieExistente.Id,
                        Nombre = superficieExistente.Nombre
                    };
                }

                logger.LogInformation("Creando superficie: {Superficie}", superficie);
                var nuevaSuperficie = new Superficie();
                nuevaSuperficie.SetNombre(superficie);

                await 
                    genericRepository.AddAsync(nuevaSuperficie, cancellationToken);
                await genericRepository.SaveChangesAsync(cancellationToken);
                transaction.Commit();

                logger.LogInformation("Superficie {Superficie} creada con Id: {Id}", superficie, nuevaSuperficie.Id);
                return new SuperficieDto
                {
                    Id = nuevaSuperficie.Id,
                    Nombre = nuevaSuperficie.Nombre
                };
            }
            catch (Exception)
            {
                string error = $"Error al crear la superficie {superficie}";
                logger.LogError(error);
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<IEnumerable<SuperficieDto>> GetAllSuperficiesAsync(CancellationToken cancellationToken = default)
    {
        var superficies = await genericRepository.GetAllAsync();
        return superficies.Select(s => new SuperficieDto() { Id = s.Id, Nombre = s.Nombre });
    }
}
