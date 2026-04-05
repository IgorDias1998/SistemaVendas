namespace SistemaVendas.Application.DTOs
{
    public class ClienteListQueryDto : PagedQueryDto
    {
        public string? Search { get; set; }
        public bool? EstaAtivo { get; set; }
    }
}
