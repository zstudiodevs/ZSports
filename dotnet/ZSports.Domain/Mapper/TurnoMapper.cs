using ZSports.Contracts.Canchas;
using ZSports.Contracts.Deportes;
using ZSports.Contracts.Establecimientos;
using ZSports.Contracts.Superficies;
using ZSports.Contracts.Turnos;
using ZSports.Contracts.Usuarios;
using ZSports.Domain.Entities;

namespace ZSports.Domain.Mapper;

public static class TurnoMapper
{
    public static TurnoDto Map(Turno source)
    {
        return new TurnoDto
        {
            Id = source.Id,
            Fecha = source.Fecha,
            HoraInicio = source.HoraInicio,
            HoraFin = source.HoraFin,
            Estado = source.Estado.ToString(),
            FechaCreacion = source.FechaCreacion,
            FechaConfirmacion = source.FechaConfirmacion,
            FechaCancelacion = source.FechaCancelacion,
            MotivoCancelacion = source.MotivoCancelacion,
            Cancha = new CanchaDto
            {
                Id = source.Cancha.Id,
                Numero = source.Cancha.Numero,
                EsIndoor = source.Cancha.EsIndoor,
                CapacidadJugadores = source.Cancha.CapacidadJugadores,
                DuracionPartido = source.Cancha.DuracionPartido,
                Activa = source.Cancha.Activa,
                Superficie = new SuperficieDto
                {
                    Id = source.Cancha.Superficie.Id,
                    Nombre = source.Cancha.Superficie.Nombre,
                    Activo = source.Cancha.Superficie.Activo
                },
                Deporte = new DeporteDto
                {
                    Id = source.Cancha.Deporte.Id,
                    Nombre = source.Cancha.Deporte.Nombre,
                    Codigo = source.Cancha.Deporte.Codigo
                },
                Establecimiento = new EstablecimientoDto
                {
                    Id = source.Cancha.Establecimiento.Id,
                    Nombre = source.Cancha.Establecimiento.Nombre,
                    Descripcion = source.Cancha.Establecimiento.Descripcion,
                    Telefono = source.Cancha.Establecimiento.Telefono,
                    Email = source.Cancha.Establecimiento.Email,
                    Activo = source.Cancha.Establecimiento.Activo,
                    Propietario = new UsuarioDto
                    {
                        Id = source.Cancha.Establecimiento.Propietario.Id,
                        Nombre = source.Cancha.Establecimiento.Propietario.Nombre,
                        Apellido = source.Cancha.Establecimiento.Propietario.Apellido,
                        Email = source.Cancha.Establecimiento.Propietario.Email!,
                        Activo = source.Cancha.Establecimiento.Propietario.Activo
                    }
                }
            },
            UsuarioCreador = new UsuarioDto
            {
                Id = source.UsuarioCreador.Id,
                Nombre = source.UsuarioCreador.Nombre,
                Apellido = source.UsuarioCreador.Apellido,
                Email = source.UsuarioCreador.Email!,
                Activo = source.UsuarioCreador.Activo
            },
            Invitaciones = source.Invitaciones.Select(i => new InvitacionTurnoDto
            {
                Id = i.Id,
                Estado = i.Estado.ToString(),
                FechaInvitacion = i.FechaInvitacion,
                FechaRespuesta = i.FechaRespuesta,
                UsuarioInvitado = new UsuarioDto
                {
                    Id = i.UsuarioInvitado.Id,
                    Nombre = i.UsuarioInvitado.Nombre,
                    Apellido = i.UsuarioInvitado.Apellido,
                    Email = i.UsuarioInvitado.Email!,
                    Activo = i.UsuarioInvitado.Activo
                }
            })
        };
    }

    public static IEnumerable<TurnoDto> MapCollection(IEnumerable<Turno> sourceCollection)
    {
        foreach (var source in sourceCollection)
        {
            yield return Map(source);
        }
    }

    public static InvitacionTurnoDto MapInvitacion(InvitacionTurno source)
    {
        return new InvitacionTurnoDto
        {
            Id = source.Id,
            Estado = source.Estado.ToString(),
            FechaInvitacion = source.FechaInvitacion,
            FechaRespuesta = source.FechaRespuesta,
            UsuarioInvitado = new UsuarioDto
            {
                Id = source.UsuarioInvitado.Id,
                Nombre = source.UsuarioInvitado.Nombre,
                Apellido = source.UsuarioInvitado.Apellido,
                Email = source.UsuarioInvitado.Email!,
                Activo = source.UsuarioInvitado.Activo
            }
        };
    }
}