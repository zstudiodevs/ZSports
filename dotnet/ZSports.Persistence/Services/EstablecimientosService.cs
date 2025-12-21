using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZSports.Contracts.Establecimientos;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Contracts.Usuarios;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Services;

public class EstablecimientosService(
    ILogger<EstablecimientosService> logger,
    IRepository<Establecimiento, Guid> repository,
    IRepository<Usuario, Guid> usuarioRepository) : IEstablecimientosService
{
    public async Task<EstablecimientoDto> CreateEstablecimientoAsync(CrearEstablecimiento crearEstablecimiento, CancellationToken cancellationToken = default)
    {
        using var transaction = await repository.BeginTransaction();
        try
        {
            logger.LogInformation("Creating new Establecimiento: {Nombre}", crearEstablecimiento.Nombre);
            var propietario = await usuarioRepository.GetByIdAsync(crearEstablecimiento.PropietarioId, cancellationToken);
            var establecimiento = new Establecimiento();
            establecimiento.SetNombre(crearEstablecimiento.Nombre);
            establecimiento.SetDescripcion(crearEstablecimiento.Descripcion);
            establecimiento.SetTelefono(crearEstablecimiento.Telefono);
            establecimiento.SetEmail(crearEstablecimiento.Email);
            establecimiento.Habilitar();
            establecimiento.SetPropietario(propietario);

            await repository.AddAsync(establecimiento, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Establecimiento created with ID: {EstablecimientoId}", establecimiento.Id);
            await transaction.CommitAsync(cancellationToken);
            return new EstablecimientoDto
            {
                Id = establecimiento.Id,
                Nombre = establecimiento.Nombre,
                Descripcion = establecimiento.Descripcion,
                Telefono = establecimiento.Telefono,
                Email = establecimiento.Email,
                Activo = establecimiento.Activo,
                Propietario = new UsuarioDto
                {
                    Id = propietario.Id,
                    Nombre = propietario.Nombre,
                    Email = propietario.Email!
                }
            };
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogError(knf.Message);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex.Message);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<IEnumerable<EstablecimientoDto>> GetAllEstablecimientosAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetQueryable()
            .Include(e => e.Propietario)
            .Select(e => new EstablecimientoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Telefono = e.Telefono,
                Email = e.Email,
                Activo = e.Activo,
                Propietario = new UsuarioDto
                {
                    Id = e.Propietario.Id,
                    Nombre = e.Propietario.Nombre,
                    Email = e.Propietario.Email!
                }
            }).ToListAsync(cancellationToken);
    }

    public async Task<EstablecimientoDto> GetEstablecimientoByIdAsync(Guid establecimientoId, CancellationToken cancellationToken = default)
    {
        return await repository.GetQueryable()
            .Include(e => e.Propietario)
            .Where(e => e.Id == establecimientoId)
            .Select(e => new EstablecimientoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Telefono = e.Telefono,
                Email = e.Email,
                Activo = e.Activo,
                Propietario = new UsuarioDto
                {
                    Id = e.Propietario.Id,
                    Nombre = e.Propietario.Nombre,
                    Email = e.Propietario.Email!
                }
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new KeyNotFoundException($"No se encontro un/a Establecimiento con Id {establecimientoId}.");
    }

    public async Task<EstablecimientoDto> GetEstablecimientoByPropietarioId(Guid propietarioId, CancellationToken cancellationToken = default)
    {
        return await repository.GetQueryable()
            .Include(e => e.Propietario)
            .Where(e => e.PropietarioId.Equals(propietarioId))
            .Select(e => new EstablecimientoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Telefono = e.Telefono,
                Email = e.Email,
                Activo = e.Activo,
                Propietario = new UsuarioDto
                {
                    Id = e.Propietario.Id,
                    Nombre = e.Propietario.Nombre,
                    Email = e.Propietario.Email!
                }
            }).FirstOrDefaultAsync(cancellationToken) ?? throw new KeyNotFoundException($"No se encontro un Establecimiento asociado al propietario con Id: {propietarioId}");
    }

    public async Task<EstablecimientoDto> UpdateEstablecimientoAsync(UpdateEstablecimiento updateEstablecimiento, CancellationToken cancellationToken = default)
    {
        using var transaction = await repository.BeginTransaction();
        try
        {
            logger.LogInformation("Actualizando Establecimiento con Id: {EstablecimientoId}", updateEstablecimiento.Id);

            // Obtener el establecimiento con el propietario
            var establecimiento = await repository.GetQueryable()
                .Include(e => e.Propietario)
                .FirstOrDefaultAsync(e => e.Id == updateEstablecimiento.Id, cancellationToken);

            if (establecimiento == null)
            {
                throw new KeyNotFoundException($"No se encontró el establecimiento con Id {updateEstablecimiento.Id}.");
            }

            // Validar que el usuario que solicita la actualización es el propietario
            if (establecimiento.PropietarioId != updateEstablecimiento.PropietarioId)
            {
                logger.LogWarning("Usuario {PropietarioId} intentó actualizar establecimiento {EstablecimientoId} sin ser propietario",
                    updateEstablecimiento.PropietarioId, updateEstablecimiento.Id);
                throw new UnauthorizedAccessException("Solo el propietario puede actualizar el establecimiento.");
            }

            // Actualizar los campos permitidos
            establecimiento.SetNombre(updateEstablecimiento.Nombre);
            establecimiento.SetDescripcion(updateEstablecimiento.Descripcion);
            establecimiento.SetTelefono(updateEstablecimiento.Telefono);
            establecimiento.SetEmail(updateEstablecimiento.Email);

            repository.Update(establecimiento);
            await repository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Establecimiento actualizado exitosamente: {EstablecimientoId}", establecimiento.Id);

            return new EstablecimientoDto
            {
                Id = establecimiento.Id,
                Nombre = establecimiento.Nombre,
                Descripcion = establecimiento.Descripcion,
                Telefono = establecimiento.Telefono,
                Email = establecimiento.Email,
                Activo = establecimiento.Activo,
                Propietario = new UsuarioDto
                {
                    Id = establecimiento.Propietario.Id,
                    Nombre = establecimiento.Propietario.Nombre,
                    Email = establecimiento.Propietario.Email!
                }
            };
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Establecimiento no encontrado al actualizar");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "Acceso no autorizado al actualizar establecimiento");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de validación al actualizar establecimiento");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado al actualizar establecimiento");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}