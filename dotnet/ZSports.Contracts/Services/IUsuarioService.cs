using ZSports.Contracts.Usuarios;

namespace ZSports.Contracts.Services;

public interface IUsuarioService
{
    Task<(bool Succeeded, IEnumerable<string> Errors)> RegistrarUsuarioAsync(RegisterUsuarioDto dto);
    Task<LoginUsuarioResponseDto?> LoginAsync(LoginUsuarioDto dto);
    Task<LoginUsuarioResponseDto?> RefreshTokenAsync(RefreshTokenDto dto);
}
