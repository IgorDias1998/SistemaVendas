using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<Delivery> AdicionarAsync(Delivery delivery);
        Task<Delivery?> BuscarPorIdAsync(Guid deliveryId);
        Task<IEnumerable<Delivery>> BuscarPorIdsAsync(IEnumerable<Guid> deliveryIds);
        Task<IEnumerable<Delivery>> BuscarTodosAsync();
        Task<IEnumerable<Delivery>> BuscarPendentesAsync();
        Task<IEnumerable<Delivery>> BuscarPorEntregadorIdAsync(Guid entregadorId);
        Task<IEnumerable<Delivery>> BuscarPendentesPorEntregadorIdAsync(Guid entregadorId);
        Task<bool> PertenceAoEntregadorAsync(Guid deliveryId, Guid entregadorId);
        Task AtualizarAsync(Delivery delivery);
    }
}
