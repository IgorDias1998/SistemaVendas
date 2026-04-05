namespace SistemaVendas.Domain.Entities
{
    public class ParadaRota
    {
        public Guid ParadaRotaId { get; set; } = Guid.NewGuid();
        public Guid RotaId { get; set; }
        public Guid DeliveryId { get; set; }
        public int StopOrder { get; set; }
        public StatusParadaRota Status { get; set; } = StatusParadaRota.Pendente;
        public DateTime? CompletoEm { get; set; }
        public Rota? Rota { get; set; }
        public Delivery? Delivery { get; set; }
    }
}
