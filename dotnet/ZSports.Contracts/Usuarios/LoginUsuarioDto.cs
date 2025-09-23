using System.ComponentModel.DataAnnotations;

namespace ZSports.Contracts.Usuarios;

public record LoginUsuarioDto
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
}
