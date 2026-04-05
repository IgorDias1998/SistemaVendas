namespace SistemaVendas.Domain.Entities
{
    public enum StatusDelivery
    {
        Pendente = 1,
        Associado = 2,
        EmRota = 3,
        Entregue = 4,
        Falhou = 5,
        Cancelado = 6
    }

    public class Delivery
    {
        public Guid DeliveryId { get; set; } = Guid.NewGuid();
        public Guid PedidoId { get; set; }
        public Guid ClienteEnderecoId { get; set; }
        public StatusDelivery Status { get; set; } = StatusDelivery.Pendente;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public Pedido? Pedido { get; set; }
        public ClienteEndereco? ClienteEndereco { get; set; }
    }
}
