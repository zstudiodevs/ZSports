namespace ZSports.Contracts.Canchas;

public record ObtenerCanchasRequest
{
    public Guid EstablecimientoId { get; init; }
    public Guid? DeporteId { get; init; }
    public bool? SoloActivas { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
