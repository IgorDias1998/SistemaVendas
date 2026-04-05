using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class PedidoListQueryDto : PagedQueryDto
    {
        public Guid? ClienteId { get; set; }
        public TipoPedido? Tipo { get; set; }
        public StatusPedido? Status { get; set; }
    }
}
