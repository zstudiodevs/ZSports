using System.ComponentModel.DataAnnotations;

namespace ZSports.Contracts.Usuarios;

public record RegisterUsuarioDto
{
    [Required, MinLength(3)] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
}
