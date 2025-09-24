using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Services;
using ZSports.Contracts.Usuarios;
using ZSports.Domain.Entities;

namespace ZSports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(
    IUsuarioService usuarioService): ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUsuarioDto registerUsuario)
    {
        var (succeeded, errors) = await usuarioService.RegistrarUsuarioAsync(registerUsuario);
        if (!succeeded)
            return BadRequest(new { errors });
        return Ok(new { message = "Usuario registrado correctamente." });

    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDto loginDto)
    {
        var result = await usuarioService.LoginAsync(loginDto);
        if (result == null)
            return Unauthorized(new { error = "Credenciales inválidas." });

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var result = await usuarioService.RefreshTokenAsync(dto);
        if (result == null)
            return Unauthorized(new { error = "Refresh token inválido o expirado." });

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        var result = await usuarioService.LogoutAsync(refreshToken);
        if (!result)
            return BadRequest(new { error = "No se pudo cerrar la sesión." });
        return Ok(new { message = "Sesión cerrada correctamente." });
    }
}
