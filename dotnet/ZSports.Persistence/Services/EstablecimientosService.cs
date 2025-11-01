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
        return repository.GetQueryable()
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
            });
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
            .Where(e => e.PropietarioId.Equals(propietarioId))
            .Select(e => new EstablecimientoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Telefono = e.Telefono,
                Email = e.Email,
                Activo = e.Activo,
            }).FirstOrDefaultAsync(cancellationToken) ?? throw new KeyNotFoundException($"No se encontro un Establecimiento asociado al propietario con Id: {propietarioId}");
    }
}
