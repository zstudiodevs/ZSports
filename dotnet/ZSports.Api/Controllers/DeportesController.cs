using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Deportes;
using ZSports.Contracts.Services;

namespace ZSports.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeportesController(
    IDeportesService service,
    ILogger<DeportesController> logger) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateDeporte request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Creando deporte con data: {data}", request);
            var result = await service.CreateDeporteAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (ArgumentException ae)
        {
            logger.LogWarning(ae, "Error de argumento al crear deporte con data: {data}", request);
            return BadRequest(ae.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error al crear deporte con data: {data}", request);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{deporteId}")]
    public async Task<IActionResult> GetByIdAsync(Guid deporteId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Obteniendo deporte con ID: {deporteId}", deporteId);
            var result = await service.GetDeporteByIdAsync(deporteId, cancellationToken);
            if (result == null)
            {
                logger.LogWarning("Deporte con ID: {deporteId} no encontrado", deporteId);
                return NotFound();
            }
            return Ok(result);
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogWarning(knf, "Deporte con ID: {deporteId} no encontrado", deporteId);
            return NotFound();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error al obtener deporte con ID: {deporteId}", deporteId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Obteniendo todos los deportes");
            var result = await service.GetAllDeportesAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error al obtener todos los deportes");
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
        }
    }
}
