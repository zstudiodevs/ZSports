using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Canchas;
using ZSports.Contracts.Services;

namespace ZSports.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CanchasController(
    ICanchasService service,
    ILogger<CanchasController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearCancha([FromBody] CreateCancha request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para crear cancha recibida");
            var result = await service.CrearCanchaAsync(request, cancellationToken);
            return CreatedAtAction(nameof(ObtenerCanchaPorId), new { id = result.Id }, result);
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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al crear cancha");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerCanchas(
        [FromQuery] Guid establecimientoId,
        [FromQuery] Guid? deporteId = null,
        [FromQuery] bool? soloActivas = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para obtener canchas recibida");

            var request = new ObtenerCanchasRequest
            {
                EstablecimientoId = establecimientoId,
                DeporteId = deporteId,
                SoloActivas = soloActivas,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await service.ObtenerCanchasAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener canchas");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerCanchaPorId(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para obtener cancha con Id: {CanchaId}", id);
            var result = await service.ObtenerCanchaPorIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Cancha no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener cancha");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpGet("{canchaId}/turnos-disponibles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerTurnosDisponibles(
        Guid canchaId,
        [FromQuery] DateOnly fecha,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para obtener turnos disponibles de cancha {CanchaId} en fecha {Fecha}",
                canchaId, fecha);

            var request = new ObtenerTurnosDisponiblesRequest
            {
                CanchaId = canchaId,
                Fecha = fecha
            };

            var result = await service.ObtenerTurnosDisponiblesAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Cancha no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener turnos disponibles");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarCancha([FromBody] UpdateCancha request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para actualizar cancha con Id: {CanchaId}", request.Id);
            var result = await service.ActualizarCanchaAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Entidad no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de validación");
            return BadRequest(new { error = ae.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al actualizar cancha");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [HttpPatch("estado")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CambiarEstadoCancha([FromBody] CambiarEstadoCancha request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para cambiar estado de cancha con Id: {CanchaId}", request.Id);
            var result = await service.CambiarEstadoCanchaAsync(request, cancellationToken);
            return Ok(new { success = result });
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Cancha no encontrada");
            return NotFound(new { error = knf.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al cambiar estado de cancha");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }
}