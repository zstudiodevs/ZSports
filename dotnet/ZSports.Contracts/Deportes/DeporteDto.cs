namespace ZSports.Contracts.Deportes;

public record DeporteDto
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Nombre { get; init; } = string.Empty;
    public string Codigo { get; init; } = string.Empty;
}
