using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class RotaListQueryDto : PagedQueryDto
    {
        public Guid? EntregadorId { get; set; }
        public StatusRota? Status { get; set; }
    }
}
