using ZSports.Contracts.Usuarios;

namespace ZSports.Contracts.Establecimientos;

public record EstablecimientoDto
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Nombre { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public bool Activo { get; init; } = true;
    public UsuarioDto Propietario { get; init; } = null!;
}
