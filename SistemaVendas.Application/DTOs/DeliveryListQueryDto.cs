using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class DeliveryListQueryDto : PagedQueryDto
    {
        public Guid? PedidoId { get; set; }
        public StatusDelivery? Status { get; set; }
    }
}
