using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Services;
using ZSports.Contracts.Turnos;

namespace ZSports.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TurnosController(
    ITurnosService service,
    ILogger<TurnosController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearTurno([FromBody] CreateTurno request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para crear turno recibida");
            var result = await service.CrearTurnoAsync(request, cancellationToken);
            return CreatedAtAction(nameof(ObtenerTurnoPorId), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Entidad relacionada no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de validación");
            return BadRequest(new { error = ae.Message });
        }
        catch (InvalidOperationException ioe)
        {
            logger.LogWarning(ioe, "Operación inválida");
            return BadRequest(new { error = ioe.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al crear turno");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerTurnoPorId(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para obtener turno con Id: {TurnoId}", id);
            var result = await service.ObtenerTurnoPorIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Turno no encontrado");
            return NotFound(new { error = knf.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener turno");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerTurnos(
        [FromQuery] Guid? canchaId,
        [FromQuery] Guid? establecimientoId,
        [FromQuery] Guid? usuarioCreadorId,
        [FromQuery] DateOnly? fechaDesde,
        [FromQuery] DateOnly? fechaHasta,
        [FromQuery] string? estado,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para obtener turnos recibida");

            var request = new ObtenerTurnosRequest
            {
                CanchaId = canchaId,
                EstablecimientoId = establecimientoId,
                UsuarioCreadorId = usuarioCreadorId,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Estado = estado,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await service.ObtenerTurnosAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener turnos");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpGet("next-by-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerProximoTurnoUsuario(
        [FromQuery] Guid usuarioId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para obtener próximo turno del usuario {UsuarioId}", usuarioId);
            var result = await service.ObtenerProximoTurnoByUsuarioAsync(usuarioId, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener próximo turno");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpGet("next-by-facility")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerProximoTurnoEstablecimiento(
        [FromQuery] Guid establecimientoId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para obtener próximo turno del establecimiento {EstablecimientoId}", establecimientoId);
            var result = await service.ObtenerProximoTurnoByEstablecimientoAsync(establecimientoId, cancellationToken);
            if (result is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { error = "No hay proximo turno." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener próximo turno");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarTurno([FromBody] UpdateTurno request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para actualizar turno con Id: {TurnoId}", request.Id);
            var result = await service.ActualizarTurnoAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Entidad no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "Acceso no autorizado");
            return Unauthorized(new { error = uae.Message });
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de validación");
            return BadRequest(new { error = ae.Message });
        }
        catch (InvalidOperationException ioe)
        {
            logger.LogWarning(ioe, "Operación inválida");
            return BadRequest(new { error = ioe.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al actualizar turno");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpPatch("confirmar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfirmarTurno([FromBody] ConfirmarTurno request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para confirmar turno {TurnoId}", request.TurnoId);
            var result = await service.ConfirmarTurnoAsync(request, cancellationToken);
            return Ok(new { success = result });
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Turno no encontrado");
            return NotFound(new { error = knf.Message });
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "Acceso no autorizado");
            return Unauthorized(new { error = uae.Message });
        }
        catch (InvalidOperationException ioe)
        {
            logger.LogWarning(ioe, "Operación inválida");
            return BadRequest(new { error = ioe.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al confirmar turno");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpPatch("cancelar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelarTurno([FromBody] CancelarTurno request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para cancelar turno {TurnoId}", request.TurnoId);
            var result = await service.CancelarTurnoAsync(request, cancellationToken);
            return Ok(new { success = result });
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Turno no encontrado");
            return NotFound(new { error = knf.Message });
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "Acceso no autorizado");
            return Unauthorized(new { error = uae.Message });
        }
        catch (InvalidOperationException ioe)
        {
            logger.LogWarning(ioe, "Operación inválida");
            return BadRequest(new { error = ioe.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al cancelar turno");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpPost("invitar")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InvitarUsuario([FromBody] InvitarUsuarioTurno request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para invitar usuario al turno");
            var result = await service.InvitarUsuarioAsync(request, cancellationToken);
            return CreatedAtAction(nameof(InvitarUsuario), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Entidad no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "Acceso no autorizado");
            return Unauthorized(new { error = uae.Message });
        }
        catch (InvalidOperationException ioe)
        {
            logger.LogWarning(ioe, "Operación inválida");
            return BadRequest(new { error = ioe.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al invitar usuario");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpPatch("invitacion/responder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResponderInvitacion([FromBody] ResponderInvitacion request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para responder invitación");
            var result = await service.ResponderInvitacionAsync(request, cancellationToken);
            return Ok(new { success = result });
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Invitación no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "Acceso no autorizado");
            return Unauthorized(new { error = uae.Message });
        }
        catch (InvalidOperationException ioe)
        {
            logger.LogWarning(ioe, "Operación inválida");
            return BadRequest(new { error = ioe.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al responder invitación");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }
}