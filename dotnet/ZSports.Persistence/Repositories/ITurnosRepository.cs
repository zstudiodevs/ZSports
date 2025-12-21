using ZSports.Domain.Entities;

namespace ZSports.Persistence.Repositories;

public interface ITurnosRepository
{
    Task<bool> ExisteSolapamientoAsync(
        Guid canchaId,
        DateOnly fecha,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        Guid? excludeTurnoId = null,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<Turno> Items, int TotalCount)> GetTurnosFiltradosAsync(
        Guid? canchaId,
        Guid? establecimientoId,
        Guid? usuarioCreadorId,
        DateOnly? fechaDesde,
        DateOnly? fechaHasta,
        string? estado,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Turno>> GetTurnosParaConfirmacionAutomaticaAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Turno>> GetTurnosParaCompletarAsync(CancellationToken cancellationToken = default);
}