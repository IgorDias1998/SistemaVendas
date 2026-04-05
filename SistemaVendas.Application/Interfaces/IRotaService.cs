using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IRotaService
    {
        Task<RotaReadDto> CriarRotaAsync(RotaCriarDto dto, Guid criadoPorUsuarioId);
        Task<IEnumerable<RotaReadDto>> BuscarRotasAsync();
        Task<IEnumerable<RotaReadDto>> BuscarRotasAsync(Guid usuarioId, string role);
        Task<PagedResultDto<RotaReadDto>> BuscarRotasAsync(Guid usuarioId, string role, RotaListQueryDto query);
        Task<RotaReadDto> BuscarRotaPorIdAsync(Guid rotaId);
        Task<RotaReadDto> BuscarRotaPorIdAsync(Guid rotaId, Guid usuarioId, string role);
        Task<RotaReadDto> AtribuirEntregadorAsync(Guid rotaId, Guid entregadorId, Guid alteradoPorUsuarioId);
        Task<RotaReadDto> ReordenarParadasAsync(Guid rotaId, RotaReordenarParadasDto dto, Guid alteradoPorUsuarioId);
        Task<RotaReadDto> ConcluirParadaAsync(Guid rotaId, Guid paradaRotaId, Guid alteradoPorUsuarioId, string role);
        Task<RotaReadDto> RegistrarFalhaParadaAsync(Guid rotaId, Guid paradaRotaId, RegistrarFalhaEntregaDto dto, Guid alteradoPorUsuarioId, string role);
        Task<RotaReadDto> IniciarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId);
        Task<RotaReadDto> IniciarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId, string role);
        Task<RotaReadDto> FinalizarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId);
        Task<RotaReadDto> FinalizarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId, string role);
        Task<IEnumerable<LogMudancaRotaReadDto>> BuscarLogsAsync(Guid rotaId, Guid usuarioId, string role);
    }
}
