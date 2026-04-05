using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoResponseDto> CriarProdutoAsync(ProdutoCriarDto produtoDto);
        Task<IEnumerable<ProdutoResponseDto>> BuscarProdutosAsync();
        Task<PagedResultDto<ProdutoResponseDto>> BuscarProdutosAsync(ProdutoListQueryDto query);
        Task<ProdutoResponseDto?> BuscarProdutoPorIdAsync(Guid produtoDtoId);
        Task<ProdutoResponseDto> AtualizarProdutoAsync(Guid id, ProdutoAtualizarDto produtoDto);
        Task DeletarProdutoAsync(Guid id);
    }
}
