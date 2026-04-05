namespace SistemaVendas.Application.DTOs
{
    public class ItemVendaReadDto
    {
        public int ItemVendaId { get; set; }
        public Guid ProdutoId { get; set; }
        public string TituloProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
