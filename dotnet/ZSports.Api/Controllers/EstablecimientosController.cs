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
            return NotFound(knf.Message);
        }
        catch (UnauthorizedAccessException uae)
        {
            logger.LogWarning(uae, "CreateEstablecimiento: Unauthorized access.");
            return Unauthorized(uae.Message);
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "CreateEstablecimiento: Bad request.");
            return BadRequest(ae.Message);
        }
        catch (Exception ex)
        {
            logger.LogError("CreateEstablecimiento: Internal server error.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
