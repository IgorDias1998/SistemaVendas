namespace SistemaVendas.Application.DTOs
{
    public class ProdutoAtualizarDto
    {
        public string TituloProduto { get; set; } = string.Empty;
        public string DescricaoProduto { get; set; } = string.Empty;
        public decimal PrecoProduto { get; set; }
        public int EstoqueProduto { get; set; }
        public string CodigoProduto { get; set; } = string.Empty;
    }
}
