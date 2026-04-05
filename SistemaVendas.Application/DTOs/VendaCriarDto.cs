namespace SistemaVendas.Application.DTOs
{
    public class VendaCriarDto
    {
        public Guid? PessoaId { get; set; }
        public DateTime? DataVenda { get; set; }
        public string Status { get; set; } = "Pendente";
        public List<ItemVendaCriarDto> ItensVenda { get; set; } = new();
    }
}
