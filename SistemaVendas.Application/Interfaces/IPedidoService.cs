using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoReadDto> CriarRascunhoAsync(PedidoCriarDto pedidoDto, Guid criadoPorUsuarioId);
        Task<IEnumerable<PedidoReadDto>> BuscarPedidosAsync();
        Task<PedidoReadDto> BuscarPedidoPorIdAsync(Guid pedidoId);
        Task<PedidoReadDto> ConfirmarPedidoAsync(Guid pedidoId);
        Task<PedidoReadDto> CancelarPedidoAsync(Guid pedidoId);
    }
}
