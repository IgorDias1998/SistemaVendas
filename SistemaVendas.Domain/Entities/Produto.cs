namespace SistemaVendas.Domain.Entities
{
    public class Produto
    {
        public Guid ProdutoId { get; private set; } = Guid.NewGuid();
        public string TituloProduto { get; private set; } = string.Empty;
        public string DescricaoProduto { get; private set; } = string.Empty;
        public decimal PrecoProduto { get; private set; }
        public int EstoqueProduto { get; private set; }
        public string CodigoProduto { get; private set; } = string.Empty;
        public ICollection<ItemVenda> ItensVenda { get; private set; } = new List<ItemVenda>();

        public Produto(string titulo, string descricao, decimal preco, int estoque, string codigo)
        {
            AtualizarDados(titulo, descricao, preco, estoque, codigo);
        }

        protected Produto() { }

        public void AtualizarDados(string titulo, string descricao, decimal preco, int estoque, string codigo)
        {
            TituloProduto = ValidarTitulo(titulo);
            DescricaoProduto = ValidarDescricao(descricao);
            PrecoProduto = ValidarPreco(preco);
            EstoqueProduto = ValidarEstoque(estoque);
            CodigoProduto = ValidarCodigo(codigo);
        }

        public void AdicionarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade adicionada deve ser maior que zero.");

            EstoqueProduto += quantidade;
        }

        public void RemoverEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade removida deve ser maior que zero.");

            if (quantidade > EstoqueProduto)
                throw new InvalidOperationException("Estoque insuficiente para a operação.");

            EstoqueProduto -= quantidade;
        }

        private static string ValidarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título do produto é obrigatório.", nameof(titulo));

            titulo = titulo.Trim();

            if (titulo.Length > 150)
                throw new ArgumentException("O título do produto deve ter no máximo 150 caracteres.", nameof(titulo));

            return titulo;
        }

        private static string ValidarDescricao(string descricao)
        {
            descricao ??= string.Empty;
            descricao = descricao.Trim();

            if (descricao.Length > 500)
                throw new ArgumentException("A descrição do produto deve ter no máximo 500 caracteres.", nameof(descricao));

            return descricao;
        }

        private static decimal ValidarPreco(decimal preco)
        {
            if (preco < 0)
                throw new ArgumentOutOfRangeException(nameof(preco), "Preço não pode ser negativo.");

            return decimal.Round(preco, 2, MidpointRounding.AwayFromZero);
        }

        private static int ValidarEstoque(int estoque)
        {
            if (estoque < 0)
                throw new ArgumentOutOfRangeException(nameof(estoque), "Estoque não pode ser negativo.");

            return estoque;
        }

        private static string ValidarCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("O código do produto é obrigatório.", nameof(codigo));

            codigo = codigo.Trim().ToUpperInvariant();

            if (codigo.Length > 20)
                throw new ArgumentException("O código do produto deve ter no máximo 20 caracteres.", nameof(codigo));

            if (!codigo.All(char.IsLetterOrDigit))
                throw new ArgumentException("O código do produto deve conter apenas letras e números.", nameof(codigo));

            return codigo;
        }
    }
}
