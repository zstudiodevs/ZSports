namespace ZSports.Contracts.Usuarios;

public record LoginUsuarioResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = [];
}
