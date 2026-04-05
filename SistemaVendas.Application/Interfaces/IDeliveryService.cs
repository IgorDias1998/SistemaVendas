using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IDeliveryService
    {
        Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesAsync();
        Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesPendentesAsync();
        Task<DeliveryReadDto> BuscarDeliveryPorIdAsync(Guid deliveryId);
        Task<DeliveryReadDto> AtualizarStatusAsync(Guid deliveryId, DeliveryAtualizarStatusDto dto);
        Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesAsync(Guid usuarioId, string role);
        Task<PagedResultDto<DeliveryReadDto>> BuscarDeliveriesAsync(Guid usuarioId, string role, DeliveryListQueryDto query);
        Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesPendentesAsync(Guid usuarioId, string role);
        Task<DeliveryReadDto> BuscarDeliveryPorIdAsync(Guid deliveryId, Guid usuarioId, string role);
        Task<DeliveryReadDto> AtualizarStatusAsync(Guid deliveryId, DeliveryAtualizarStatusDto dto, Guid usuarioId, string role);
        Task<DeliveryReadDto> RegistrarFalhaAsync(Guid deliveryId, RegistrarFalhaEntregaDto dto, Guid usuarioId, string role);
    }
}
