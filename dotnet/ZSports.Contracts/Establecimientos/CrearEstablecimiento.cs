namespace ZSports.Contracts.Establecimientos;

public record CrearEstablecimiento
{
    public string Nombre { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public bool Activo { get; init; } = true;
    public Guid PropietarioId { get; init; } = Guid.Empty;
}
