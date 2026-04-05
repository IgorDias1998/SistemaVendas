using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class RotaParadaReadDto
    {
        public Guid ParadaRotaId { get; set; }
        public Guid DeliveryId { get; set; }
        public int StopOrder { get; set; }
        public StatusParadaRota Status { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string EnderecoResumo { get; set; } = string.Empty;
    }
}
