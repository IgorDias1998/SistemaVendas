using SistemaVendas.Application.DTOs;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IProdutoService
    {
        public Task CriarProduto(ProdutoCriarDto produtoDto);
        public Task<IEnumerable<ProdutoResponseDto>> BuscarProdutosAsync();
        public Task<ProdutoResponseDto?> BuscarProdutoPorIdAsync(Guid produtoDtoId);
        public Task AtualizarProdutoAsync(Guid Id, ProdutoAtualizarDto pessoa);
        public Task DeletarProdutoAsync(Guid id);
    }
}
