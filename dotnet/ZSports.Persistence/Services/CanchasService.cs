using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZSports.Contracts.Canchas;
using ZSports.Contracts.Common;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Domain.Entities;
using ZSports.Domain.Mapper;
using ZSports.Persistence.Repositories;

namespace ZSports.Persistence.Services;

public class CanchasService(
    ILogger<CanchasService> logger,
    IRepository<Cancha, Guid> repository,
    ICanchasRepository canchasRepository,
    IRepository<Superficie, Guid> superficieRepository,
    IRepository<Deporte, Guid> deporteRepository,
    IRepository<Establecimiento, Guid> establecimientoRepository) : ICanchasService
{
    public async Task<CanchaDto> CrearCanchaAsync(CreateCancha request, CancellationToken cancellationToken = default)
    {
        using var transaction = await repository.BeginTransaction();
        try
        {
            logger.LogInformation("Iniciando creación de cancha número {Numero} para establecimiento {EstablecimientoId}",
                request.Numero, request.EstablecimientoId);

            // Validar que no exista el número de cancha en el establecimiento
            var existeNumero = await canchasRepository.ExisteNumeroCanchaEnEstablecimientoAsync(
                request.Numero,
                request.EstablecimientoId,
                null,
                cancellationToken);

            if (existeNumero)
            {
                throw new ArgumentException($"Ya existe una cancha con el número {request.Numero} en este establecimiento.");
            }

            // Validar que existan las entidades relacionadas
            var superficie = await superficieRepository.GetByIdAsync(request.SuperficieId, cancellationToken);
            var deporte = await deporteRepository.GetByIdAsync(request.DeporteId, cancellationToken);
            var establecimiento = await establecimientoRepository.GetByIdAsync(request.EstablecimientoId, cancellationToken);

            // Crear la cancha
            var cancha = new Cancha();
            cancha.SetNumero(request.Numero);
            cancha.SetEsIndoor(request.EsIndoor);
            cancha.SetCapacidadJugadores(request.CapacidadJugadores);
            cancha.SetDuracionPartido(request.DuracionPartido);
            cancha.SetSuperficie(superficie);
            cancha.SetDeporte(deporte);
            cancha.SetEstablecimiento(establecimiento);

            if (request.Activa)
                cancha.Activar();
            else
                cancha.Desactivar();

            await repository.AddAsync(cancha, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Cancha creada exitosamente con Id: {CanchaId}", cancha.Id);

            return CanchaMapper.Map(cancha);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Entidad relacionada no encontrada al crear cancha");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de validación al crear cancha");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado al crear cancha");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<PaginatedResponse<CanchaDto>> ObtenerCanchasAsync(
        ObtenerCanchasRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Obteniendo canchas para establecimiento {EstablecimientoId}", request.EstablecimientoId);

            var (items, totalCount) = await canchasRepository.GetCanchasFiltradas(
                request.EstablecimientoId,
                request.DeporteId,
                request.SoloActivas,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var canchasDto = CanchaMapper.MapCollection(items);

            return new PaginatedResponse<CanchaDto>
            {
                Items = canchasDto,
                TotalItems = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener canchas filtradas");
            throw;
        }
    }

    public async Task<CanchaDto> ObtenerCanchaPorIdAsync(Guid canchaId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Obteniendo cancha con Id: {CanchaId}", canchaId);

            var cancha = await repository.GetQueryable()
                .Include(c => c.Superficie)
                .Include(c => c.Deporte)
                .Include(c => c.Establecimiento)
                    .ThenInclude(e => e.Propietario)
                .FirstOrDefaultAsync(c => c.Id == canchaId, cancellationToken);

            if (cancha == null)
            {
                throw new KeyNotFoundException($"No se encontró la cancha con Id {canchaId}.");
            }

            return CanchaMapper.Map(cancha);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Cancha no encontrada");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener cancha por Id");
            throw;
        }
    }

    public async Task<CanchaDto> ActualizarCanchaAsync(UpdateCancha request, CancellationToken cancellationToken = default)
    {
        using var transaction = await repository.BeginTransaction();
        try
        {
            logger.LogInformation("Actualizando cancha con Id: {CanchaId}", request.Id);

            // Obtener la cancha con sus relaciones
            var cancha = await repository.GetQueryable()
                .Include(c => c.Superficie)
                .Include(c => c.Deporte)
                .Include(c => c.Establecimiento)
                    .ThenInclude(e => e.Propietario)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (cancha == null)
            {
                throw new KeyNotFoundException($"No se encontró la cancha con Id {request.Id}.");
            }

            // Validar que la cancha pertenece al establecimiento especificado
            if (cancha.EstablecimientoId != request.EstablecimientoId)
            {
                throw new ArgumentException("La cancha no pertenece al establecimiento especificado.");
            }

            // Validar número único si cambió
            if (cancha.Numero != request.Numero)
            {
                var existeNumero = await canchasRepository.ExisteNumeroCanchaEnEstablecimientoAsync(
                    request.Numero,
                    request.EstablecimientoId,
                    request.Id,
                    cancellationToken);

                if (existeNumero)
                {
                    throw new ArgumentException($"Ya existe una cancha con el número {request.Numero} en este establecimiento.");
                }

                cancha.SetNumero(request.Numero);
            }

            // Actualizar propiedades
            cancha.SetEsIndoor(request.EsIndoor);
            cancha.SetCapacidadJugadores(request.CapacidadJugadores);
            cancha.SetDuracionPartido(request.DuracionPartido);

            // Actualizar relaciones si cambiaron
            if (cancha.SuperficieId != request.SuperficieId)
            {
                var superficie = await superficieRepository.GetByIdAsync(request.SuperficieId, cancellationToken);
                cancha.SetSuperficie(superficie);
            }

            if (cancha.DeporteId != request.DeporteId)
            {
                var deporte = await deporteRepository.GetByIdAsync(request.DeporteId, cancellationToken);
                cancha.SetDeporte(deporte);
            }

            repository.Update(cancha);
            await repository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Cancha actualizada exitosamente: {CanchaId}", cancha.Id);

            return CanchaMapper.Map(cancha);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Entidad no encontrada al actualizar cancha");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de validación al actualizar cancha");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado al actualizar cancha");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> CambiarEstadoCanchaAsync(CambiarEstadoCancha request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Cambiando estado de cancha {CanchaId} a {Estado}",
                request.Id, request.Activa ? "Activa" : "Inactiva");

            var cancha = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (request.Activa)
                cancha.Activar();
            else
                cancha.Desactivar();

            repository.Update(cancha);
            await repository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Estado de cancha cambiado exitosamente");

            return true;
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Cancha no encontrada al cambiar estado");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al cambiar estado de cancha");
            throw;
        }
    }
}