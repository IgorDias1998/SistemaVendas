using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class VendaReadDto
    {
        public int VendaId { get; set; }
        public Guid? ClienteId { get; set; }
        public DateTime? DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public VendaStatus Status { get; set; }
        public List<ItemVendaReadDto> ItensVenda { get; set; } = new();
    }
}
