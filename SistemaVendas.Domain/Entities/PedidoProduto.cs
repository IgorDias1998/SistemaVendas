namespace SistemaVendas.Domain.Entities
{
    public class PedidoProduto
    {
        public Guid PedidoProdutoId { get; set; } = Guid.NewGuid();
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public Pedido? Pedido { get; set; }
        public Produto? Produto { get; set; }
    }
}
