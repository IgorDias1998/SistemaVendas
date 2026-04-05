using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<Delivery> AdicionarAsync(Delivery delivery);
        Task AtualizarAsync(Delivery delivery);
    }
}
