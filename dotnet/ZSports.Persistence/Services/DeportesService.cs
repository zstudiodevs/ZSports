using Microsoft.Extensions.Logging;
using ZSports.Contracts.Deportes;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Domain.Entities;
using ZSports.Domain.Mapper;

namespace ZSports.Persistence.Services;

public class DeportesService(
    IRepository<Deporte, Guid> repository,
    ILogger<DeportesService> logger): IDeportesService
{
    public async Task<DeporteDto> CreateDeporteAsync(CreateDeporte request, CancellationToken cancellationToken = default)
    {
        using var transaction = await repository.BeginTransaction();
		try
		{
            var deporte = new Deporte();
            deporte.SetNombre(request.Nombre);
            deporte.SetCodigo(request.Codigo);
            deporte.Habilitar();

            await repository.AddAsync(deporte, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return DeporteMapper.Map(deporte);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e.Message);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
		catch (Exception e)
		{
            logger.LogError(e.Message);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<DeporteDto> GetDeporteByIdAsync(Guid deporteId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Buscando deporte con Id: {DeporteId}", deporteId);
            var deporte = await repository.GetByIdAsync(deporteId, cancellationToken);
            logger.LogInformation("Deporte encontrado: {Deporte}", deporte.Nombre);
            return DeporteMapper.Map(deporte);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
        catch (Exception)
        {
            logger.LogError("Error al buscar deporte con Id: {DeporteId}", deporteId);
            throw;
        }
    }

    public async Task<IEnumerable<DeporteDto>> GetAllDeportesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var deportes = await repository.GetAllAsync(cancellationToken);
            return DeporteMapper.MapCollection(deportes);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}
