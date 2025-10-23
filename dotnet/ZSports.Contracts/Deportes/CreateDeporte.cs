namespace ZSports.Contracts.Deportes;

public record CreateDeporte
{
    public string Nombre { get; init; } = string.Empty;
    public string Codigo { get; init; } = string.Empty;
}
