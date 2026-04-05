using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class LogMudancaRotaReadDto
    {
        public Guid LogMudancaRotaId { get; set; }
        public Guid RotaId { get; set; }
        public Guid AlteradoPeloUsuarioId { get; set; }
        public DateTime MudouEm { get; set; }
        public TipoMudancaRota TipoMudanca { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
    }
}
