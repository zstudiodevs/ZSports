using ZSports.Domain.Constants;

namespace ZSports.Domain.Entities;

public class Establecimiento
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public string Telefono { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool Activo { get; private set; } = true;
    public Guid PropietarioId { get; private set; } = Guid.Empty;
    public virtual Usuario Propietario { get; private set; } = null!;

    public void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre no puede estar vacío.");
        }

        if (nombre.Length > EstablecimientosConstants.MaxNombreLength)
        {
            throw new ArgumentException($"El nombre no puede exceder {EstablecimientosConstants.MaxNombreLength} caracteres.");
        }

        Nombre = nombre;
    }

    public void SetDescripcion(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
        {
            descripcion = string.Empty;
        }

        if (descripcion.Length > EstablecimientosConstants.MaxDescripcionLength)
        {
            throw new ArgumentException($"La descripción no puede exceder {EstablecimientosConstants.MaxDescripcionLength} caracteres.");
        }

        Descripcion = descripcion;
    }

    public void SetTelefono(string telefono)
    {
        if (string.IsNullOrEmpty(telefono))
        {
            throw new ArgumentException("El teléfono no puede estar vacío.");
        }
        if (telefono.Length > EstablecimientosConstants.MaxTelefonoLength)
        {
            throw new ArgumentException($"El teléfono no puede exceder {EstablecimientosConstants.MaxTelefonoLength} caracteres.");
        }
        Telefono = telefono;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("El email no puede estar vacío.");
        }

        if (email.Length > EstablecimientosConstants.MaxEmailLength)
        {
            throw new ArgumentException($"El email no puede exceder {EstablecimientosConstants.MaxEmailLength} caracteres.");
        }

        Email = email;
    }

    public void Habilitar()
    {
        Activo = true;
    }

    public void Deshabilitar()
    {
        Activo = false;
    }

    public void SetPropietario(Usuario propietario)
    {
        Propietario = propietario ?? throw new ArgumentNullException(nameof(propietario), "El propietario no puede ser nulo.");
        PropietarioId = propietario.Id;
    }
}
