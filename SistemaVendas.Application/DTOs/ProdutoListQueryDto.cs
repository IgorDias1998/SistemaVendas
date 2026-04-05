namespace SistemaVendas.Application.DTOs
{
    public class ProdutoListQueryDto : PagedQueryDto
    {
        public string? Search { get; set; }
    }
}
