using ZSports.Contracts.Canchas;
using ZSports.Contracts.Deportes;
using ZSports.Contracts.Establecimientos;
using ZSports.Contracts.Superficies;
using ZSports.Contracts.Usuarios;
using ZSports.Domain.Entities;

namespace ZSports.Domain.Mapper;

public static class CanchaMapper
{
    public static CanchaDto Map(Cancha source)
    {
        return new CanchaDto
        {
            Id = source.Id,
            Numero = source.Numero,
            EsIndoor = source.EsIndoor,
            CapacidadJugadores = source.CapacidadJugadores,
            DuracionPartido = source.DuracionPartido,
            Activa = source.Activa,
            Superficie = new SuperficieDto
            {
                Id = source.Superficie.Id,
                Nombre = source.Superficie.Nombre,
                Activo = source.Superficie.Activo
            },
            Deporte = new DeporteDto
            {
                Id = source.Deporte.Id,
                Nombre = source.Deporte.Nombre,
                Codigo = source.Deporte.Codigo
            },
            Establecimiento = new EstablecimientoDto
            {
                Id = source.Establecimiento.Id,
                Nombre = source.Establecimiento.Nombre,
                Descripcion = source.Establecimiento.Descripcion,
                Telefono = source.Establecimiento.Telefono,
                Email = source.Establecimiento.Email,
                Activo = source.Establecimiento.Activo,
                Propietario = new UsuarioDto
                {
                    Id = source.Establecimiento.Propietario.Id,
                    Nombre = source.Establecimiento.Propietario.Nombre,
                    Apellido = source.Establecimiento.Propietario.Apellido,
                    Email = source.Establecimiento.Propietario.Email!,
                    Activo = source.Establecimiento.Propietario.Activo
                }
            }
        };
    }

    public static IEnumerable<CanchaDto> MapCollection(IEnumerable<Cancha> sourceCollection)
    {
        foreach (var source in sourceCollection)
        {
            yield return Map(source);
        }
    }
}