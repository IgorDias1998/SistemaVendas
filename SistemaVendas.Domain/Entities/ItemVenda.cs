namespace SistemaVendas.Domain.Entities
{
    public class ItemVenda
    {
        public int ItemVendaId { get; set; }
        public int VendaId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public Venda? Venda { get; set; }
        public Produto Produto { get; set; }
    }
}
