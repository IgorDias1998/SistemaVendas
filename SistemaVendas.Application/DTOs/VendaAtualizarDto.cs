namespace SistemaVendas.Application.DTOs
{
    public class VendaAtualizarDto
    {
        public Guid? PessoaId { get; set; }
        public DateTime? DataVenda { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
