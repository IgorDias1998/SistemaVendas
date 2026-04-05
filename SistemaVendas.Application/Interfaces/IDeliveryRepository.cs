using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<Delivery> AdicionarAsync(Delivery delivery);
        Task<Delivery?> BuscarPorIdAsync(Guid deliveryId);
        Task<IEnumerable<Delivery>> BuscarTodosAsync();
        Task<IEnumerable<Delivery>> BuscarPendentesAsync();
        Task AtualizarAsync(Delivery delivery);
    }
}
