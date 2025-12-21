using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZSports.Contracts.Common;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Contracts.Turnos;
using ZSports.Domain.Entities;
using ZSports.Domain.Enums;
using ZSports.Domain.Mapper;
using ZSports.Persistence.Repositories;

namespace ZSports.Persistence.Services;

public class TurnosService(
    ILogger<TurnosService> logger,
    IRepository<Turno, Guid> turnoRepository,
    ITurnosRepository turnosRepository,
    IRepository<Cancha, Guid> canchaRepository,
    IRepository<Usuario, Guid> usuarioRepository,
    IRepository<InvitacionTurno, Guid> invitacionRepository) : ITurnosService
{
    public async Task<TurnoDto> CrearTurnoAsync(CreateTurno request, CancellationToken cancellationToken = default)
    {
        using var transaction = await turnoRepository.BeginTransaction();
        try
        {
            logger.LogInformation("Creando turno para cancha {CanchaId} en fecha {Fecha}", request.CanchaId, request.Fecha);

            // Obtener cancha con establecimiento
            var cancha = await canchaRepository.GetQueryable()
                .Include(c => c.Establecimiento)
                .Include(c => c.Superficie)
                .Include(c => c.Deporte)
                .Include(c => c.Establecimiento.Propietario)
                .FirstOrDefaultAsync(c => c.Id == request.CanchaId, cancellationToken);

            if (cancha == null)
            {
                throw new KeyNotFoundException($"No se encontró la cancha con Id {request.CanchaId}.");
            }

            // Obtener usuario creador
            var usuario = await usuarioRepository.GetByIdAsync(request.UsuarioCreadorId, cancellationToken);

            // Crear turno
            var turno = new Turno();
            turno.SetFecha(request.Fecha);
            turno.SetHorarios(request.HoraInicio, cancha.DuracionPartido, cancha.Establecimiento.HoraInicioMinima, cancha.Establecimiento.HoraFinMaxima);
            turno.SetCancha(cancha);
            turno.SetUsuarioCreador(usuario);

            // Validar que no haya solapamiento
            var existeSolapamiento = await turnosRepository.ExisteSolapamientoAsync(
                cancha.Id,
                turno.Fecha,
                turno.HoraInicio,
                turno.HoraFin,
                null,
                cancellationToken);

            if (existeSolapamiento)
            {
                throw new InvalidOperationException("Ya existe un turno en ese horario para la cancha seleccionada.");
            }

            await turnoRepository.AddAsync(turno, cancellationToken);
            await turnoRepository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Turno creado exitosamente con Id: {TurnoId}", turno.Id);

            return TurnoMapper.Map(turno);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Entidad relacionada no encontrada al crear turno");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de validación al crear turno");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (InvalidOperationException ioe)
        {
            logger.LogWarning(ioe, "Operación inválida al crear turno");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado al crear turno");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<TurnoDto> ObtenerTurnoPorIdAsync(Guid turnoId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Obteniendo turno con Id: {TurnoId}", turnoId);

            var turno = await turnoRepository.GetQueryable()
                .Include(t => t.Cancha)
                    .ThenInclude(c => c.Superficie)
                .Include(t => t.Cancha)
                    .ThenInclude(c => c.Deporte)
                .Include(t => t.Cancha)
                    .ThenInclude(c => c.Establecimiento)
                        .ThenInclude(e => e.Propietario)
                .Include(t => t.UsuarioCreador)
                .Include(t => t.Invitaciones)
                    .ThenInclude(i => i.UsuarioInvitado)
                .FirstOrDefaultAsync(t => t.Id == turnoId, cancellationToken);

            if (turno == null)
            {
                throw new KeyNotFoundException($"No se encontró el turno con Id {turnoId}.");
            }

            return TurnoMapper.Map(turno);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener turno por Id");
            throw;
        }
    }

    public async Task<PaginatedResponse<TurnoDto>> ObtenerTurnosAsync(ObtenerTurnosRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Obteniendo turnos con filtros");

            var (items, totalCount) = await turnosRepository.GetTurnosFiltradosAsync(
                request.CanchaId,
                request.EstablecimientoId,
                request.UsuarioCreadorId,
                request.FechaDesde,
                request.FechaHasta,
                request.Estado,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var turnosDto = TurnoMapper.MapCollection(items);

            return new PaginatedResponse<TurnoDto>
            {
                Items = turnosDto,
                TotalItems = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener turnos filtrados");
            throw;
        }
    }

    public async Task<TurnoDto> ActualizarTurnoAsync(UpdateTurno request, CancellationToken cancellationToken = default)
    {
        using var transaction = await turnoRepository.BeginTransaction();
        try
        {
            logger.LogInformation("Actualizando turno con Id: {TurnoId}", request.Id);

            var turno = await turnoRepository.GetQueryable()
                .Include(t => t.Cancha)
                    .ThenInclude(c => c.Establecimiento)
                        .ThenInclude(e => e.Propietario)
                .Include(t => t.Cancha.Superficie)
                .Include(t => t.Cancha.Deporte)
                .Include(t => t.UsuarioCreador)
                .Include(t => t.Invitaciones)
                    .ThenInclude(i => i.UsuarioInvitado)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (turno == null)
            {
                throw new KeyNotFoundException($"No se encontró el turno con Id {request.Id}.");
            }

            // Validar que el usuario que actualiza es el creador
            if (turno.UsuarioCreadorId != request.UsuarioCreadorId)
            {
                throw new UnauthorizedAccessException("Solo el creador del turno puede modificarlo.");
            }

            // Actualizar fecha
            turno.SetFecha(request.Fecha);

            // Si cambió la cancha, validar que sea del mismo establecimiento
            if (turno.CanchaId != request.CanchaId)
            {
                var nuevaCancha = await canchaRepository.GetQueryable()
                    .Include(c => c.Establecimiento)
                    .FirstOrDefaultAsync(c => c.Id == request.CanchaId, cancellationToken);

                if (nuevaCancha == null)
                {
                    throw new KeyNotFoundException($"No se encontró la cancha con Id {request.CanchaId}.");
                }

                if (nuevaCancha.EstablecimientoId != turno.Cancha.EstablecimientoId)
                {
                    throw new InvalidOperationException("No se puede cambiar el turno a una cancha de otro establecimiento.");
                }

                turno.SetCancha(nuevaCancha);
            }

            // Actualizar horarios
            turno.ActualizarHorarios(
                request.HoraInicio,
                turno.Cancha.DuracionPartido,
                turno.Cancha.Establecimiento.HoraInicioMinima,
                turno.Cancha.Establecimiento.HoraFinMaxima);

            // Validar que no haya solapamiento
            var existeSolapamiento = await turnosRepository.ExisteSolapamientoAsync(
                turno.CanchaId,
                turno.Fecha,
                turno.HoraInicio,
                turno.HoraFin,
                turno.Id,
                cancellationToken);

            if (existeSolapamiento)
            {
                throw new InvalidOperationException("Ya existe un turno en ese horario para la cancha seleccionada.");
            }

            turnoRepository.Update(turno);
            await turnoRepository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Turno actualizado exitosamente: {TurnoId}", turno.Id);

            return TurnoMapper.Map(turno);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al actualizar turno");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> ConfirmarTurnoAsync(ConfirmarTurno request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Confirmando turno {TurnoId}", request.TurnoId);

            var turno = await turnoRepository.GetByIdAsync(request.TurnoId, cancellationToken);

            if (turno.UsuarioCreadorId != request.UsuarioId)
            {
                throw new UnauthorizedAccessException("Solo el creador del turno puede confirmarlo.");
            }

            turno.Confirmar();
            turnoRepository.Update(turno);
            await turnoRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Turno confirmado exitosamente");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al confirmar turno");
            throw;
        }
    }

    public async Task<bool> CancelarTurnoAsync(CancelarTurno request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Cancelando turno {TurnoId}", request.TurnoId);

            var turno = await turnoRepository.GetByIdAsync(request.TurnoId, cancellationToken);

            if (turno.UsuarioCreadorId != request.UsuarioId)
            {
                throw new UnauthorizedAccessException("Solo el creador del turno puede cancelarlo.");
            }

            turno.Cancelar(request.Motivo);
            turnoRepository.Update(turno);
            await turnoRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Turno cancelado exitosamente");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al cancelar turno");
            throw;
        }
    }

    public async Task<InvitacionTurnoDto> InvitarUsuarioAsync(InvitarUsuarioTurno request, CancellationToken cancellationToken = default)
    {
        using var transaction = await turnoRepository.BeginTransaction();
        try
        {
            logger.LogInformation("Invitando usuario {UsuarioId} al turno {TurnoId}", request.UsuarioInvitadoId, request.TurnoId);

            var turno = await turnoRepository.GetQueryable()
                .Include(t => t.Cancha)
                .Include(t => t.Invitaciones)
                .FirstOrDefaultAsync(t => t.Id == request.TurnoId, cancellationToken);

            if (turno == null)
            {
                throw new KeyNotFoundException($"No se encontró el turno con Id {request.TurnoId}.");
            }

            if (turno.UsuarioCreadorId != request.UsuarioCreadorId)
            {
                throw new UnauthorizedAccessException("Solo el creador del turno puede invitar usuarios.");
            }

            if (turno.Estado == EstadoTurno.Cancelado || turno.Estado == EstadoTurno.Completado)
            {
                throw new InvalidOperationException("No se pueden agregar invitaciones a un turno cancelado o completado.");
            }

            var usuarioInvitado = await usuarioRepository.GetByIdAsync(request.UsuarioInvitadoId, cancellationToken);

            // Validar que no esté ya invitado
            if (turno.Invitaciones.Any(i => i.UsuarioInvitadoId == request.UsuarioInvitadoId))
            {
                throw new InvalidOperationException("El usuario ya está invitado a este turno.");
            }

            var invitacion = new InvitacionTurno();
            invitacion.SetTurno(turno);
            invitacion.SetUsuarioInvitado(usuarioInvitado);

            await invitacionRepository.AddAsync(invitacion, cancellationToken);
            await invitacionRepository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Usuario invitado exitosamente");

            return TurnoMapper.MapInvitacion(invitacion);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al invitar usuario");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> ResponderInvitacionAsync(ResponderInvitacion request, CancellationToken cancellationToken = default)
    {
        using var transaction = await invitacionRepository.BeginTransaction();
        try
        {
            logger.LogInformation("Respondiendo invitación {InvitacionId}", request.InvitacionId);

            var invitacion = await invitacionRepository.GetQueryable()
                .Include(i => i.Turno)
                    .ThenInclude(t => t.Cancha)
                .Include(i => i.Turno.Invitaciones)
                .FirstOrDefaultAsync(i => i.Id == request.InvitacionId, cancellationToken);

            if (invitacion == null)
            {
                throw new KeyNotFoundException($"No se encontró la invitación con Id {request.InvitacionId}.");
            }

            if (invitacion.UsuarioInvitadoId != request.UsuarioInvitadoId)
            {
                throw new UnauthorizedAccessException("Solo el usuario invitado puede responder la invitación.");
            }

            if (request.Aceptar)
            {
                invitacion.Aceptar();

                // Verificar si debe confirmarse automáticamente
                if (invitacion.Turno.DebeConfirmarseAutomaticamente(invitacion.Turno.Cancha.CapacidadJugadores))
                {
                    invitacion.Turno.Confirmar();
                    turnoRepository.Update(invitacion.Turno);
                    logger.LogInformation("Turno confirmado automáticamente por alcanzar capacidad");
                }
            }
            else
            {
                invitacion.Rechazar();
            }

            invitacionRepository.Update(invitacion);
            await invitacionRepository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Invitación respondida exitosamente");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al responder invitación");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<TurnoDto> ObtenerProximoTurnoByUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        using var transaction = await turnoRepository.BeginTransaction();
        try
        {
            logger.LogInformation("Obteniendo próximo turno para el usuario {UsuarioId}", usuarioId);
            var turno = await turnoRepository.GetQueryable()
                .Include(t => t.Cancha)
                    .ThenInclude(c => c.Establecimiento)
                        .ThenInclude(e => e.Propietario)
                .Include(t => t.Cancha.Superficie)
                .Include(t => t.Cancha.Deporte)
                .Include(t => t.UsuarioCreador)
                .Include(t => t.Invitaciones)
                    .ThenInclude(i => i.UsuarioInvitado)
                .Where(t => t.UsuarioCreadorId == usuarioId && t.Estado == EstadoTurno.Confirmado &&
                            (t.Fecha > DateOnly.FromDateTime(DateTime.UtcNow) ||
                             (t.Fecha == DateOnly.FromDateTime(DateTime.UtcNow) && t.HoraInicio > DateTime.UtcNow.TimeOfDay)))
                .OrderBy(t => t.Fecha)
                .ThenBy(t => t.HoraInicio)
                .FirstOrDefaultAsync(cancellationToken);

            return turno is null
                ? throw new KeyNotFoundException($"No se encontró un próximo turno para el usuario con Id {usuarioId}.")
                : TurnoMapper.Map(turno);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener próximo turno por usuario");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<TurnoDto?> ObtenerProximoTurnoByEstablecimientoAsync(Guid establicmientoId, CancellationToken cancellationToken = default)
    {
        using var transaction = await turnoRepository.BeginTransaction();
        try
        {
            logger.LogInformation("Obteniendo próximo turno para el establecimiento {EstablecimientoId}", establicmientoId);
            var turno = await turnoRepository.GetQueryable()
                .Include(t => t.Cancha)
                    .ThenInclude(c => c.Establecimiento)
                        .ThenInclude(e => e.Propietario)
                .Include(t => t.Cancha.Superficie)
                .Include(t => t.Cancha.Deporte)
                .Include(t => t.UsuarioCreador)
                .Include(t => t.Invitaciones)
                    .ThenInclude(i => i.UsuarioInvitado)
                .Where(t => t.Cancha.EstablecimientoId == establicmientoId && (t.Estado == EstadoTurno.Confirmado || t.Estado == EstadoTurno.Pendiente)&&
                            (t.Fecha > DateOnly.FromDateTime(DateTime.UtcNow) ||
                             (t.Fecha == DateOnly.FromDateTime(DateTime.UtcNow) && t.HoraInicio > DateTime.UtcNow.TimeOfDay)))
                .OrderBy(t => t.Fecha)
                .ThenBy(t => t.HoraInicio)
                .FirstOrDefaultAsync(cancellationToken);

            return turno is null
                ? null
                : TurnoMapper.Map(turno);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener el proximo turno para el establecimiento");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}