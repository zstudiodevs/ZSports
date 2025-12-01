namespace ZSports.Contracts.Usuarios;

public record AddRoleToUserRequest
{
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
