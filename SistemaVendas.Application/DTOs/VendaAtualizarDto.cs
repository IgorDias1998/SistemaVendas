using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class VendaAtualizarDto
    {
        public Guid? ClienteId { get; set; }
        public DateTime? DataVenda { get; set; }
        public VendaStatus Status { get; set; } = VendaStatus.Rascunho;
    }
}
