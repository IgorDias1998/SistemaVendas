using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class RotaReadDto
    {
        public Guid RotaId { get; set; }
        public Guid CriadoPeloUsuarioId { get; set; }
        public Guid? AssociadoAoEntregadorId { get; set; }
        public StatusRota Status { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AtribuidoEm { get; set; }
        public DateTime? InicioEm { get; set; }
        public DateTime? TerminoEm { get; set; }
        public List<RotaParadaReadDto> Paradas { get; set; } = new();
    }
}
