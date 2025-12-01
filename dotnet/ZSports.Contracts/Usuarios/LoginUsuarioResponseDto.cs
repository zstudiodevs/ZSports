namespace ZSports.Contracts.Usuarios;

public record LoginUsuarioResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public UsuarioDto Usuario { get; set; } = null!;
}
