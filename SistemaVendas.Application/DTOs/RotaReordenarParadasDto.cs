namespace SistemaVendas.Application.DTOs
{
    public class RotaReordenarParadasDto
    {
        public Guid AlteradoPeloUsuarioId { get; set; }
        public List<Guid> ParadaIdsEmOrdem { get; set; } = new();
    }
}
