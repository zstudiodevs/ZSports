namespace ZSports.Contracts.Establecimientos;

public record UpdateEstablecimiento
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid PropietarioId { get; init; } // Para validar que el que actualiza es el propietario
}