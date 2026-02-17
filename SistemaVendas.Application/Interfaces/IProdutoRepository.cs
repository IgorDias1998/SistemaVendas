using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IProdutoRepository
    {
        public Task AdicionarProdutoAsync(Produto produto);
        public Task<IEnumerable<Produto>> BuscarProdutosAsync();
        public Task<Produto?> BuscarProdutoPorIdAsync(Guid produtoId);
        public Task AtualizarProdutoAsync(Produto pessoa);
        public Task DeletarProdutoAsync(Guid produtoId);
    }
}
