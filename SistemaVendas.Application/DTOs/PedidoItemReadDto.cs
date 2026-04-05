namespace SistemaVendas.Application.DTOs
{
    public class PedidoItemReadDto
    {
        public Guid PedidoProdutoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string TituloProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
    }
}
