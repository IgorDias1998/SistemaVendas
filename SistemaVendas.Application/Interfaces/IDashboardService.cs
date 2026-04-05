using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardReadDto> ObterDashboardAsync(Guid usuarioId, string role);
    }
}
