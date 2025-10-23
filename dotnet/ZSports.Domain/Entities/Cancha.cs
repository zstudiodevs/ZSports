namespace ZSports.Domain.Entities;

public class Cancha
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int Numero { get; private set; }
    public bool EsIndoor { get; private set; } = true;
    public int CapacidadJugadores { get; private set; }
    public int DuracionPartido { get; private set; }
    public bool Activa { get; private set; } = true;
    public Guid SuperficieId { get; private set; }
    public virtual Superficie Superficie { get; private set; } = null!;
    public Guid DeporteId { get; private set; }
    public virtual Deporte Deporte { get; private set; } = null!;
    public Guid EstablecimientoId { get; private set; }
    public virtual Establecimiento Establecimiento { get; private set; } = null!;

    public void SetNumero(int numero)
    {
        if (numero <= 0)
        {
            throw new ArgumentException("El número de la cancha debe ser mayor que cero.");
        }

        Numero = numero;
    }

    public void SetEsIndoor(bool esIndoor)
    {
        EsIndoor = esIndoor;
    }

    public void SetCapacidadJugadores(int capacidadJugadores)
    {
        if (capacidadJugadores <= 0)
        {
            throw new ArgumentException("La capacidad de jugadores debe ser mayor que cero.");
        }

        CapacidadJugadores = capacidadJugadores;
    }

    public void SetDuracionPartido(int duracionPartido)
    {
        if (duracionPartido <= 0)
        {
            throw new ArgumentException("La duración del partido debe ser mayor que cero.");
        }

        DuracionPartido = duracionPartido;
    }

    public void Activar()
    {
        Activa = true;
    }

    public void Desactivar()
    {
        Activa = false;
    }

    public void SetSuperficie(Superficie superficie)
    {
        Superficie = superficie ?? throw new ArgumentNullException(nameof(superficie), "La superficie no puede ser nula.");
        SuperficieId = superficie.Id;
    }

    public void SetDeporte(Deporte deporte)
    {
        Deporte = deporte ?? throw new ArgumentNullException(nameof(deporte), "El deporte no puede ser nulo.");
        DeporteId = deporte.Id;
    }

    public void SetEstablecimiento(Establecimiento establecimiento)
    {
        Establecimiento = establecimiento ?? throw new ArgumentNullException(nameof(establecimiento), "El establecimiento no puede ser nulo.");
        EstablecimientoId = establecimiento.Id;
    }
}
