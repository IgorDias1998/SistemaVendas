namespace SistemaVendas.Domain.Entities
{
    public enum StatusRota
    {
        Rascunho = 1,
        Atribuida = 2,
        EmProgresso = 3,
        Finalizada = 4,
        Cancelada = 5
    }

    public enum StatusParadaRota
    {
        Pendente = 1,
        Realizado = 2,
        PulouPedido = 3
    }

    public class Rota
    {
        public Guid RotaId { get; set; } = Guid.NewGuid();
        public Guid CriadoPeloUsuarioId { get; set; }
        public Guid? AssociadoAoEntregadorId { get; set; }
        public StatusRota Status { get; set; } = StatusRota.Rascunho;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? AtribuidoEm { get; set; }
        public DateTime? InicioEm { get; set; }
        public DateTime? TerminoEm { get; set; }
        public Usuario? CriadoPeloUsuario { get; set; }
        public Usuario? Entregador { get; set; }
        public ICollection<ParadaRota> Paradas { get; set; } = new List<ParadaRota>();
    }
}
