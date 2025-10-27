namespace ZSports.Contracts.Establecimientos;

public record CrearEstablecimiento
{
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public string Telefono { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool Activo { get; private set; } = true;
    public Guid PropietarioId { get; private set; } = Guid.Empty;
}
