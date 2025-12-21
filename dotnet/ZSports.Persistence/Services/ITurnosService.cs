using ZSports.Contracts.Common;
using ZSports.Contracts.Turnos;

namespace ZSports.Contracts.Services;

public interface ITurnosService
{
    Task<TurnoDto> CrearTurnoAsync(CreateTurno request, CancellationToken cancellationToken = default);
    Task<TurnoDto> ObtenerTurnoPorIdAsync(Guid turnoId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<TurnoDto>> ObtenerTurnosAsync(ObtenerTurnosRequest request, CancellationToken cancellationToken = default);
    Task<TurnoDto> ObtenerProximoTurnoByUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<TurnoDto?> ObtenerProximoTurnoByEstablecimientoAsync(Guid establicmientoId, CancellationToken cancellationToken = default);
    Task<TurnoDto> ActualizarTurnoAsync(UpdateTurno request, CancellationToken cancellationToken = default);
    Task<bool> ConfirmarTurnoAsync(ConfirmarTurno request, CancellationToken cancellationToken = default);
    Task<bool> CancelarTurnoAsync(CancelarTurno request, CancellationToken cancellationToken = default);
    Task<InvitacionTurnoDto> InvitarUsuarioAsync(InvitarUsuarioTurno request, CancellationToken cancellationToken = default);
    Task<bool> ResponderInvitacionAsync(ResponderInvitacion request, CancellationToken cancellationToken = default);
}