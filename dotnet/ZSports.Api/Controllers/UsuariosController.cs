using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZSports.Contracts.Services;
using ZSports.Contracts.Usuarios;

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

    [HttpGet("validate-token")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateToken(CancellationToken cancellationToken = default)
    {
        // Si el token es válido, el usuario estará autenticado
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await usuarioService.GetUserByUsername(User.Identity.Name!, cancellationToken);
            if (user == null)
                return Unauthorized(new { valid = false } );
            if (!user.Activo)
                return Unauthorized(new { valid = false, error = "El usuario no está activo." } );

            return Ok(new
            {
                valid = true,
                username = User.Identity.Name,
                usuario = user,
                error = string.Empty
            });
        }

        return Unauthorized(new { valid = false });
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
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var result = await usuarioService.LogoutAsync(request.RefreshToken);
        if (!result)
            return BadRequest(new { error = "No se pudo cerrar la sesión." });
        return Ok(new { message = "Sesión cerrada correctamente." });
    }

    [HttpPut("update")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateUsuario([FromBody] UpdateUsuarioDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var updatedUser = await usuarioService.UpdateUsuarioAsync(request, cancellationToken);
            return Ok(updatedUser);
        }
        catch (Exception)
        {
            return BadRequest(new { error = "No se pudo actualizar el usuario." });
            throw;
        }
    }
}
