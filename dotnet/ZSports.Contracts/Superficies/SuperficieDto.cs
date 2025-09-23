namespace ZSports.Contracts.Superficies;

public record SuperficieDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Nombre { get; set; } = string.Empty;
}
