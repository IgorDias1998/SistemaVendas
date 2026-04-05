namespace SistemaVendas.Application.DTOs
{
    public class VendaReadDto
    {
        public int VendaId { get; set; }
        public Guid? PessoaId { get; set; }
        public DateTime? DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<ItemVendaReadDto> ItensVenda { get; set; } = new();
    }
}
