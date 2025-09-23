namespace ZSports.Domain.Entities;

public class Superficie
{
    public Guid Id{ get; private set; } = Guid.NewGuid();
    public string Nombre { get; private set; } = string.Empty;

    public void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.");
        if (nombre.Length > 100)
            throw new ArgumentException("El nombre no puede tener más de 100 caracteres.");
        Nombre = nombre;
    }
}
