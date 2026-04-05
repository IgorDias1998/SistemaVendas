using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class PedidoCriarDto
    {
        public Guid ClienteId { get; set; }
        public Guid CriadoPeloUsuarioId { get; set; }
        public TipoPedido Tipo { get; set; }
        public string? Observacao { get; set; }
        public List<PedidoItemCriarDto> Itens { get; set; } = new();
    }
}
