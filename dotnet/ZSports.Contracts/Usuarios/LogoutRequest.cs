namespace ZSports.Contracts.Usuarios;

public record LogoutRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}
