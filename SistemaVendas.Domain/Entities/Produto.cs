namespace SistemaVendas.Domain.Entities
{
    public class Produto
    {
        public Guid ProdutoId { get; set; }
        public string TituloProduto { get; set; } = string.Empty;
        public string DescricaoProduto { get; set; } = string.Empty;
        public decimal PrecoProduto { get; set; }
        public int EstoqueProduto { get; set; }
        public string CodigoProduto { get; set; } = string.Empty;

        public Produto(string titulo, string descricao, decimal preco, int estoque, string codigo)
        {
            if (preco < 0)
                throw new Exception("Preço não pode ser negativo.");

            if (estoque < 0)
                throw new Exception("Estoque não pode ser negativo.");

            TituloProduto = titulo;
            DescricaoProduto = descricao;
            PrecoProduto = preco;
            EstoqueProduto = estoque;
            CodigoProduto = codigo;
        }
    }
}
