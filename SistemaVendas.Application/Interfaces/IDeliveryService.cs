using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IDeliveryService
    {
        Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesAsync();
        Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesPendentesAsync();
        Task<DeliveryReadDto> BuscarDeliveryPorIdAsync(Guid deliveryId);
        Task<DeliveryReadDto> AtualizarStatusAsync(Guid deliveryId, DeliveryAtualizarStatusDto dto);
    }
}
