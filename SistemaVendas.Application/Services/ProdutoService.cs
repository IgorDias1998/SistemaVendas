using System.Globalization;
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
            {
                throw new CultureNotFoundException(nameof(produto));
            }

            return MapearParaResponse(produto);
        }

        public async Task<IEnumerable<ProdutoResponseDto>> BuscarProdutosAsync()
        {
            var produtos = await _repository.BuscarProdutosAsync();

            return produtos.Select(MapearParaResponse).ToList();
        }

        public async Task CriarProduto(ProdutoCriarDto produtoDto)
        {
            var produto = new Produto
            {
                ProdutoId = Guid.NewGuid(),
                TituloProduto = produtoDto.TituloProduto,
                DescricaoProduto = produtoDto.DescricaoProduto,
                PrecoProduto = produtoDto.PrecoProduto,
                EstoqueProduto = produtoDto.EstoqueProduto,
                CodigoProduto = produtoDto.CodigoProduto
            };

            await _repository.AdicionarProdutoAsync(produto); 
        }

        public async Task AtualizarProdutoAsync(Guid Id, ProdutoAtualizarDto produtoDto)
        {
            var produto = await _repository.BuscarProdutoPorIdAsync(Id);

            if (produto is null)
                throw new Exception("Produto não encontrado..");

            produto.TituloProduto = produtoDto.TituloProduto;
            produto.DescricaoProduto = produtoDto.DescricaoProduto;
            produto.PrecoProduto = produtoDto.PrecoProduto;
            produto.EstoqueProduto = produtoDto.EstoqueProduto;
            produto.CodigoProduto = produtoDto.CodigoProduto;

            await _repository.AtualizarProdutoAsync(produto);
        }

        public async Task DeletarProdutoAsync(Guid id)
        {
            var produto = await _repository.BuscarProdutoPorIdAsync(id);

            if (produto is null)
                throw new Exception("Produto não encontrado..");

            await _repository.DeletarProdutoAsync(produto.ProdutoId);
        }

        private ProdutoResponseDto MapearParaResponse(Produto produto)
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
