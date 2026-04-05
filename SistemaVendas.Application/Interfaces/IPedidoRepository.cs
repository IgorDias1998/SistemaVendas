using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido> AdicionarAsync(Pedido pedido);
        Task<Pedido?> BuscarPorIdAsync(Guid pedidoId);
        Task<IEnumerable<Pedido>> BuscarTodosAsync();
        Task AtualizarAsync(Pedido pedido);
    }
}
