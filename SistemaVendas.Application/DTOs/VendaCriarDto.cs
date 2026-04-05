using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class VendaCriarDto
    {
        public Guid? ClienteId { get; set; }
        public DateTime? DataVenda { get; set; }
        public VendaStatus Status { get; set; } = VendaStatus.Confirmado;
        public List<ItemVendaCriarDto> ItensVenda { get; set; } = new();
    }
}
