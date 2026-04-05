using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IRotaService
    {
        Task<RotaReadDto> CriarRotaAsync(RotaCriarDto dto);
        Task<IEnumerable<RotaReadDto>> BuscarRotasAsync();
        Task<RotaReadDto> BuscarRotaPorIdAsync(Guid rotaId);
        Task<RotaReadDto> AtribuirEntregadorAsync(Guid rotaId, Guid entregadorId);
        Task<RotaReadDto> ReordenarParadasAsync(Guid rotaId, RotaReordenarParadasDto dto);
        Task<RotaReadDto> IniciarRotaAsync(Guid rotaId);
        Task<RotaReadDto> FinalizarRotaAsync(Guid rotaId);
    }
}
