using Microsoft.EntityFrameworkCore;
using ZSports.Domain.Entities;
using ZSports.Domain.Enums;

namespace ZSports.Persistence.Repositories;

public class TurnosRepository(ZSportsDbContext dbContext) : ITurnosRepository
{
    public async Task<bool> ExisteSolapamientoAsync(
        Guid canchaId,
        DateOnly fecha,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        Guid? excludeTurnoId = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Turno>()
            .Where(t => t.CanchaId == canchaId
                && t.Fecha == fecha
                && t.Estado != EstadoTurno.Cancelado);

        if (excludeTurnoId.HasValue)
        {
            query = query.Where(t => t.Id != excludeTurnoId.Value);
        }

        // Verificar solapamiento con margen de 10 minutos
        var margen = TimeSpan.FromMinutes(10);
        var horaInicioConMargen = horaInicio.Subtract(margen);
        var horaFinConMargen = horaFin.Add(margen);

        return await query.AnyAsync(t =>
            (t.HoraInicio < horaFinConMargen && t.HoraFin > horaInicioConMargen),
            cancellationToken);
    }

    public async Task<(IEnumerable<Turno> Items, int TotalCount)> GetTurnosFiltradosAsync(
        Guid? canchaId,
        Guid? establecimientoId,
        Guid? usuarioCreadorId,
        DateOnly? fechaDesde,
        DateOnly? fechaHasta,
        string? estado,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Turno>()
            .Include(t => t.Cancha)
                .ThenInclude(c => c.Superficie)
            .Include(t => t.Cancha)
                .ThenInclude(c => c.Deporte)
            .Include(t => t.Cancha)
                .ThenInclude(c => c.Establecimiento)
                    .ThenInclude(e => e.Propietario)
            .Include(t => t.UsuarioCreador)
            .Include(t => t.Invitaciones)
                .ThenInclude(i => i.UsuarioInvitado)
            .AsQueryable();

        if (canchaId.HasValue)
        {
            query = query.Where(t => t.CanchaId == canchaId.Value);
        }

        if (establecimientoId.HasValue)
        {
            query = query.Where(t => t.Cancha.EstablecimientoId == establecimientoId.Value);
        }

        if (usuarioCreadorId.HasValue)
        {
            query = query.Where(t => t.UsuarioCreadorId == usuarioCreadorId.Value);
        }

        if (fechaDesde.HasValue)
        {
            query = query.Where(t => t.Fecha >= fechaDesde.Value);
        }

        if (fechaHasta.HasValue)
        {
            query = query.Where(t => t.Fecha <= fechaHasta.Value);
        }

        if (!string.IsNullOrEmpty(estado) && Enum.TryParse<EstadoTurno>(estado, out var estadoEnum))
        {
            query = query.Where(t => t.Estado == estadoEnum);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(t => t.Fecha)
            .ThenBy(t => t.HoraInicio)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IEnumerable<Turno>> GetTurnosParaConfirmacionAutomaticaAsync(CancellationToken cancellationToken = default)
    {
        var ahora = DateTime.Now;
        var fechaHoy = DateOnly.FromDateTime(ahora);
        var horaActual = ahora.TimeOfDay;
        var unaHoraDespues = horaActual.Add(TimeSpan.FromHours(1));

        return await dbContext.Set<Turno>()
            .Include(t => t.Cancha)
            .Include(t => t.Invitaciones)
            .Where(t => t.Estado == EstadoTurno.Pendiente
                && t.Fecha == fechaHoy
                && t.HoraInicio <= unaHoraDespues
                && t.HoraInicio > horaActual)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Turno>> GetTurnosParaCompletarAsync(CancellationToken cancellationToken = default)
    {
        var ahora = DateTime.Now;
        var fechaHoy = DateOnly.FromDateTime(ahora);
        var horaActual = ahora.TimeOfDay;

        return await dbContext.Set<Turno>()
            .Where(t => (t.Estado == EstadoTurno.Confirmado || t.Estado == EstadoTurno.Pendiente)
                && ((t.Fecha < fechaHoy) || (t.Fecha == fechaHoy && t.HoraFin <= horaActual)))
            .ToListAsync(cancellationToken);
    }
}