using ZSports.Contracts.Usuarios;

namespace ZSports.Contracts.Services;

public interface IUsuarioService
{
    Task<(bool Succeeded, IEnumerable<string> Errors)> RegistrarUsuarioAsync(RegisterUsuarioDto dto);
    Task<(LoginUsuarioResponseDto? Response, string Error)> LoginAsync(LoginUsuarioDto dto);
    Task<LoginUsuarioResponseDto?> RefreshTokenAsync(RefreshTokenDto dto);
    Task<bool> LogoutAsync(string refreshToken);
    Task<UsuarioDto> GetUserByUsername(string username, CancellationToken cancellationToken = default);
    Task<UsuarioDto> UpdateUsuarioAsync(UpdateUsuarioDto request, CancellationToken cancellationToken = default);
    Task<(bool Succeeded, IEnumerable<string> Errors)> ChangePasswordAsync(ChangePasswordDto dto);
    Task<(bool Succeeded, IEnumerable<string> Errors)> AddRoleToUser(string email, string role);
}
