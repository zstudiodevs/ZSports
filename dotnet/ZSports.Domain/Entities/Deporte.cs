using ZSports.Domain.Constants;

namespace ZSports.Domain.Entities;

public class Deporte
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nombre { get; private set; } = string.Empty;
    public string Codigo { get; private set; } = string.Empty;
    public bool Activo { get; private set; } = true;

    public void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentNullException("El nombre no puede estar vacío.");
        }

        if (nombre.Length > DeportesConstants.MaxNombreLength)
        {
            throw new ArgumentException($"El nombre no puede exceder {DeportesConstants.MaxNombreLength} caracteres.");
        }

        Nombre = nombre;
    }

    public void SetCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
        {
            throw new ArgumentNullException("El código no puede estar vacío.");
        }
        if (codigo.Length > DeportesConstants.MaxCodigoLength)
        {
            throw new ArgumentException($"El código no puede exceder {DeportesConstants.MaxCodigoLength} caracteres.");
        }

        Codigo = codigo;
    }

    public void Habilitar()
    {
        Activo = true;
    }

    public void Deshabilitar()
    {
        Activo = false;
    }
}
