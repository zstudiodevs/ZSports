using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZSports.Domain.Entities;

public class Usuario: IdentityUser<Guid>
{
    public string Nombre { get; private set; } = string.Empty;
    public string Apellido { get; private set; } = string.Empty;
    public DateTime FechaAlta { get; private set; } = DateTime.UtcNow;
    public DateTime? UltimoAcceso { get; private set; } = null;
    public bool Activo { get; private set; } = true;

    public void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
        if (nombre.Length > 100)
            throw new ArgumentException("El nombre no puede tener más de 100 caracteres.", nameof(nombre));

        Nombre = nombre.Trim();
    }

    public void SetApellido(string apellido)
    {
        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido no puede estar vacío.", nameof(apellido));
        if (apellido.Length > 100)
            throw new ArgumentException("El apellido no puede tener más de 100 caracteres.", nameof(apellido));
        Apellido = apellido.Trim();
    }

    public void ActualizarUltimoAcceso()
    {
        UltimoAcceso = DateTime.UtcNow;
    }
}
