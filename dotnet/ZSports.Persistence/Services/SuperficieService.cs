using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Contracts.Superficies;
using ZSports.Domain.Entities;
using ZSports.Domain.Mapper;
using ZSports.Persistence.Repositories;

namespace ZSports.Persistence.Services;

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
                    return superficieExistente;
                }

                logger.LogInformation("Creando superficie: {Superficie}", superficie);
                var nuevaSuperficie = new Superficie();
                nuevaSuperficie.SetNombre(superficie);

                await 
                    genericRepository.AddAsync(nuevaSuperficie, cancellationToken);
                await genericRepository.SaveChangesAsync(cancellationToken);
                transaction.Commit();

                logger.LogInformation("Superficie {Superficie} creada con Id: {Id}", superficie, nuevaSuperficie.Id);
                return SuperficieMapper.Map(nuevaSuperficie);
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

    public async Task<IEnumerable<SuperficieDto>> GetAllAsync(bool includeDisabled = false, CancellationToken cancellationToken = default)
    {
        var superficies = genericRepository.GetQueryable();
        if (includeDisabled)
        {
            superficies = superficies.Where(s => s.Activo);
        }
        return SuperficieMapper.MapCollection(await superficies.ToListAsync(cancellationToken));
    }

    public async Task<SuperficieDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var superficie = await genericRepository.GetByIdAsync(id, cancellationToken);
        if (superficie is null)
            return null;
        return SuperficieMapper.Map(superficie);
    }

    public async Task<SuperficieDto> UpdateAsync(SuperficieDto superficieDto, CancellationToken cancellationToken = default)
    {
        using(var transaction = await genericRepository.BeginTransaction())
        {
            try
            {
                logger.LogInformation("Iniciando el proceso de actualizacion de superficie con Id: {Id}", superficieDto.Id);
                var superficieExistente = await genericRepository.GetByIdAsync(superficieDto.Id, cancellationToken);
                if (superficieExistente is null)
                {
                    string error = $"No se encontro la superficie con Id: {superficieDto.Id}";
                    logger.LogError(error);
                    throw new KeyNotFoundException(error);
                }
                logger.LogInformation("Actualizando superficie con Id: {Id}", superficieDto.Id);
                superficieExistente.SetNombre(superficieDto.Nombre);
                genericRepository.Update(superficieExistente);
                await genericRepository.SaveChangesAsync(cancellationToken);
                transaction.Commit();
                logger.LogInformation("Superficie con Id: {Id} actualizada correctamente", superficieDto.Id);
                return SuperficieMapper.Map(superficieExistente);
            }
            catch (Exception)
            {
                string error = $"Error al actualizar la superficie con Id: {superficieDto.Id}";
                logger.LogError(error);
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<bool> EnableAsync(Guid superficieId, CancellationToken cancellationToken = default)
    {
        var superficie = await genericRepository.GetByIdAsync(superficieId, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontro la superficie con Id: {superficieId}");            

        superficie.Habilitar();
        genericRepository.Update(superficie);
        await genericRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DisableAsync(Guid superficieId, CancellationToken cancellationToken = default)
    {
        var superficie = await genericRepository.GetByIdAsync(superficieId, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontro la superficie con Id: {superficieId}");            
        superficie.Deshabilitar();
        genericRepository.Update(superficie);
        await genericRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
