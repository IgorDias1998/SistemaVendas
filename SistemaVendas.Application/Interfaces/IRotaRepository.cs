using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IRotaRepository
    {
        Task<Rota> AdicionarAsync(Rota rota);
        Task<Rota?> BuscarPorIdAsync(Guid rotaId);
        Task<IEnumerable<Rota>> BuscarTodosAsync();
        Task AtualizarAsync(Rota rota);
        Task<bool> AlgumaDeliveryEmRotaAtivaAsync(IEnumerable<Guid> deliveryIds);
    }
}
