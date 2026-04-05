using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class DeliveryReadDto
    {
        public Guid DeliveryId { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ClienteEnderecoId { get; set; }
        public StatusDelivery Status { get; set; }
        public DateTime CriadoEm { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
    }
}
