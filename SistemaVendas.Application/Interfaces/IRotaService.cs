using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IRotaService
    {
        Task<RotaReadDto> CriarRotaAsync(RotaCriarDto dto, Guid criadoPorUsuarioId);
        Task<IEnumerable<RotaReadDto>> BuscarRotasAsync();
        Task<RotaReadDto> BuscarRotaPorIdAsync(Guid rotaId);
        Task<RotaReadDto> AtribuirEntregadorAsync(Guid rotaId, Guid entregadorId, Guid alteradoPorUsuarioId);
        Task<RotaReadDto> ReordenarParadasAsync(Guid rotaId, RotaReordenarParadasDto dto, Guid alteradoPorUsuarioId);
        Task<RotaReadDto> IniciarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId);
        Task<RotaReadDto> FinalizarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId);
    }
}
