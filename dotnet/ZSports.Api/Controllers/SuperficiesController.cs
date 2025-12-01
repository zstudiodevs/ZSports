using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Services;
using ZSports.Contracts.Superficies;

namespace ZSports.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuperficiesController(
    ILogger<SuperficiesController> logger,
    ISuperficieService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSuperficieDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var superficie = await service.CreateAsync(request.Superficie, cancellationToken);
            return Created(nameof(CreateAsync), superficie);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating superficie");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the superficie.");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync([FromQuery]bool includeDisabled = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var superficies = await service.GetAllAsync(includeDisabled, cancellationToken);
            return Ok(superficies);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving superficies");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the superficies.");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var superficie = await service.GetByIdAsync(id, cancellationToken);
            if (superficie == null)
                return NotFound();
            return Ok(superficie);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving superficie with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error intentando obtener la superficie.");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromBody] SuperficieDto superficieDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.UpdateAsync(superficieDto, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating superficie with ID {Id}", superficieDto.Id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the superficie.");
        }
    }

    [HttpPatch("enable/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EnableAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.EnableAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "No se encontro la Superficie con Id: {Id}", id);
            return BadRequest($"No se encontro la Superficie con Id: {id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocurrio un error durante la habilitacion de una Superficie con Id: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while enabling the superficie.");
        }
    }

    [HttpPatch("disable/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DisableAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.DisableAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "No se encontro la Superficie con Id: {Id}", id);
            return BadRequest($"No se encontro la Superficie con Id: {id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocurrio un error durante la deshabilitacion de una Superficie con Id: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while disabling the superficie.");
        }
    }
}
