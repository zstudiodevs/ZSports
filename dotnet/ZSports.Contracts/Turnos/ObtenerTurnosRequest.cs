namespace ZSports.Contracts.Turnos;

public record ObtenerTurnosRequest
{
    public Guid? CanchaId { get; init; }
    public Guid? EstablecimientoId { get; init; }
    public Guid? UsuarioCreadorId { get; init; }
    public DateOnly? FechaDesde { get; init; }
    public DateOnly? FechaHasta { get; init; }
    public string? Estado { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}