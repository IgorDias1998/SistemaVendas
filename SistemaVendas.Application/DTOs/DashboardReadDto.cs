namespace SistemaVendas.Application.DTOs
{
    public class DashboardReadDto
    {
        public int TotalProdutos { get; set; }
        public int TotalClientes { get; set; }
        public int PedidosRascunho { get; set; }
        public int PedidosConfirmados { get; set; }
        public int DeliveriesPendentes { get; set; }
        public int DeliveriesEmRota { get; set; }
        public int RotasRascunho { get; set; }
        public int RotasEmProgresso { get; set; }
        public int MinhasRotas { get; set; }
        public int MinhasDeliveriesPendentes { get; set; }
    }
}
