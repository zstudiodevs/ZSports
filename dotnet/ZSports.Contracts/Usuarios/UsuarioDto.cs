namespace ZSports.Contracts.Usuarios;

public record UsuarioDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public IEnumerable<string> Roles { get; set; } = [];
}
