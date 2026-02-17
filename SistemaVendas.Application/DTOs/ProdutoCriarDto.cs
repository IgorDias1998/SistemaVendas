namespace SistemaVendas.Application.DTOs
{
    public class ProdutoCriarDto
    {
        public string TituloProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public decimal PrecoProduto { get; set; }
        public int EstoqueProduto { get; set; }
        public string CodigoProduto { get; set; }
    }
}
