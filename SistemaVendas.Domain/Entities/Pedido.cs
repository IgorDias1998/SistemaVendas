namespace SistemaVendas.Domain.Entities
{
    public enum TipoPedido
    {
        Delivery = 1,
        Retirada = 2
    }

    public enum StatusPedido
    {
        Rascunho = 1,
        Confirmado = 2,
        Cancelado = 3,
        Completo = 4
    }

    public class Pedido
    {
        public Guid PedidoId { get; set; } = Guid.NewGuid();
        public Guid ClienteId { get; set; }
        public Guid CriadoPeloUsuarioId { get; set; }
        public TipoPedido Tipo { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.Rascunho;
        public string? Observacao { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmadoEm { get; set; }
        public Cliente? Cliente { get; set; }
        public Usuario? CriadoPeloUsuario { get; set; }
        public ICollection<PedidoProduto> Itens { get; set; } = new List<PedidoProduto>();
        public Delivery? Delivery { get; set; }
    }
}
