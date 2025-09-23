using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Services;

namespace ZSports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController(IRolesService roleService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return BadRequest("El nombre del rol es requerido.");

        var created = await roleService.CreateRoleAsync(roleName);
        if (!created)
            return BadRequest("El rol ya existe.");

        return Ok("Rol creado correctamente.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await roleService.GetAllRolesAsync();
        return Ok(roles);
    }
}
