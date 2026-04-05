using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class PedidoReadDto
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public Guid CriadoPeloUsuarioId { get; set; }
        public TipoPedido Tipo { get; set; }
        public StatusPedido Status { get; set; }
        public string? Observacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? ConfirmadoEm { get; set; }
        public Guid? DeliveryId { get; set; }
        public List<PedidoItemReadDto> Itens { get; set; } = new();
    }
}
