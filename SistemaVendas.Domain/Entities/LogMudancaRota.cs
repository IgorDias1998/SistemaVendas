namespace SistemaVendas.Domain.Entities
{
    public enum TipoMudancaRota
    {
        Reordenar = 1,
        Atribuir = 2,
        Iniciar = 3,
        Finalizar = 4
    }

    public class LogMudancaRota
    {
        public Guid LogMudancaRotaId { get; set; } = Guid.NewGuid();
        public Guid RotaId { get; set; }
        public Guid AlteradoPeloUsuarioId { get; set; }
        public DateTime MudouEm { get; set; } = DateTime.UtcNow;
        public TipoMudancaRota TipoMudanca { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public Rota? Rota { get; set; }
        public Usuario? AlteradoPeloUsuario { get; set; }
    }
}
