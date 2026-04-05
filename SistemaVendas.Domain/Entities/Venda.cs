namespace SistemaVendas.Domain.Entities
{
    public enum VendaStatus
    {
        Rascunho = 1,
        Confirmado = 2,
        Cancelado = 3,
        Finalizado = 4
    }

    public class Venda
    {
        public int VendaId { get; set; }
        public Guid? ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public VendaStatus Status { get; set; } = VendaStatus.Rascunho;
        public Cliente? Cliente { get; set; }
        public ICollection<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
    }
}
