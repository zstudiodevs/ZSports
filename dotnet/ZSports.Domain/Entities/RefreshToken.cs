namespace ZSports.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool Revoked { get; set; } = false;
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}
