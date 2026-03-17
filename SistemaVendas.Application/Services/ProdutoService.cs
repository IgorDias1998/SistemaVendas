using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProdutoResponseDto?> BuscarProdutoPorIdAsync(Guid produtoDtoId)
        {
            var produto = await _repository.BuscarProdutoPorIdAsync(produtoDtoId);

            if (produto is null)
                throw new KeyNotFoundException("Produto não encontrado.");

            return MapearParaResponse(produto);
        }

        public async Task<IEnumerable<ProdutoResponseDto>> BuscarProdutosAsync()
        {
            var produtos = await _repository.BuscarProdutosAsync();

            return produtos.Select(MapearParaResponse).ToList();
        }

        public async Task<ProdutoResponseDto> CriarProdutoAsync(ProdutoCriarDto produtoDto)
        {
            var produto = new Produto(
                produtoDto.TituloProduto,
                produtoDto.DescricaoProduto,
                produtoDto.PrecoProduto,
                produtoDto.EstoqueProduto,
                produtoDto.CodigoProduto
            );

            await _repository.AdicionarProdutoAsync(produto);

            return MapearParaResponse(produto);
        }

        public async Task<ProdutoResponseDto> AtualizarProdutoAsync(Guid id, ProdutoAtualizarDto produtoDto)
        {
            var produto = await _repository.BuscarProdutoPorIdAsync(id);

            if (produto is null)
                throw new KeyNotFoundException("Produto não encontrado.");

            produto.AtualizarDados(
                produtoDto.TituloProduto,
                produtoDto.DescricaoProduto,
                produtoDto.PrecoProduto,
                produtoDto.EstoqueProduto,
                produtoDto.CodigoProduto
            );

            await _repository.AtualizarProdutoAsync(produto);

            return MapearParaResponse(produto);
        }

        public async Task DeletarProdutoAsync(Guid id)
        {
            var produto = await _repository.BuscarProdutoPorIdAsync(id);

            if (produto is null)
                throw new KeyNotFoundException("Produto não encontrado.");

            await _repository.DeletarProdutoAsync(produto.ProdutoId);
        }

        private static ProdutoResponseDto MapearParaResponse(Produto produto)
        {
            return new ProdutoResponseDto
            {
                ProdutoId = produto.ProdutoId,
                TituloProduto = produto.TituloProduto,
                DescricaoProduto = produto.DescricaoProduto,
                PrecoProduto = produto.PrecoProduto,
                EstoqueProduto = produto.EstoqueProduto,
                CodigoProduto = produto.CodigoProduto
            };
        }
    }
}
