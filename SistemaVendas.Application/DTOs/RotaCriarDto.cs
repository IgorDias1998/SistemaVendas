namespace SistemaVendas.Application.DTOs
{
    public class RotaCriarDto
    {
        public Guid? EntregadorId { get; set; }
        public List<Guid> DeliveryIds { get; set; } = new();
    }
}
