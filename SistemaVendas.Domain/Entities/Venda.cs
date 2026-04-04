namespace SistemaVendas.Domain.Entities
{
    public class Venda
    {
        public int VendaId { get; set; }
        public Guid? PessoaId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public Pessoa? Pessoa { get; set; }
        public ICollection<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
    }
}
