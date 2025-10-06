using Microsoft.AspNetCore.Http;
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
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var superficies = await service.GetAllAsync(cancellationToken);
            return Ok(superficies);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving superficies");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the superficies.");
        }
    }

}
