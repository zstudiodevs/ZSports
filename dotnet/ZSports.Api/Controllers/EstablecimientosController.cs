using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Establecimientos;
using ZSports.Contracts.Services;

namespace ZSports.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstablecimientosController(
    IEstablecimientosService service,
    ILogger<EstablecimientosController> logger) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> CreateEstablecimiento([FromBody] CrearEstablecimiento request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.CreateEstablecimientoAsync(request, cancellationToken);
            return CreatedAtAction(nameof(CreateEstablecimiento), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "CreateEstablecimiento: Related resource not found.");
            return NotFound(new { error = knf.Message });
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "CreateEstablecimiento: Unauthorized access.");
            return Unauthorized(new { error = uae.Message });
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "CreateEstablecimiento: Bad request.");
            return BadRequest(new { error = ae.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateEstablecimiento: Internal server error.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{propietarioId}")]
    public async Task<IActionResult> GetEstablecimientoByPropietarioId(Guid propietarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.GetEstablecimientoByPropietarioId(propietarioId, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "GetEstablecimientoByPropietarioId: Establecimiento not found.");
            return NotFound(new { error = knf.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetEstablecimientoByPropietarioId: Internal server error.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetEstablecimientos(CancellationToken cancellationToken)
    {
        try
        {
            var result = await service.GetAllEstablecimientosAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetEstablecimientos: Internal server error.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> UpdateEstablecimiento([FromBody] UpdateEstablecimiento request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Solicitud para actualizar establecimiento con Id: {EstablecimientoId}", request.Id);
            var result = await service.UpdateEstablecimientoAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "UpdateEstablecimiento: Establecimiento not found.");
            return NotFound(new { error = knf.Message });
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "UpdateEstablecimiento: Unauthorized access.");
            return Unauthorized(new { error = uae.Message });
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "UpdateEstablecimiento: Bad request.");
            return BadRequest(new { error = ae.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateEstablecimiento: Internal server error.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Ocurrió un error al procesar la solicitud." });
        }
    }
}