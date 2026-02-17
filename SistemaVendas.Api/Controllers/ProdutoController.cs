using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Application.Services;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarProduto([FromBody] ProdutoCriarDto produtoDto)
        {
            await _produtoService.CriarProduto(produtoDto);
            return Ok("Produto criado com sucesso..");
        }

        [HttpGet]
        public async Task<ActionResult> BuscarTodosProdutos()
        {
            var produtos = await _produtoService.BuscarProdutosAsync();

            return Ok(produtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarProdutoPorId(Guid id)
        {
            var produto = await _produtoService.BuscarProdutoPorIdAsync(id);

            return Ok(produto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> AtualizarProduto(Guid id, [FromBody] ProdutoAtualizarDto produtoAtualizarDto)
        {
            await _produtoService.AtualizarProdutoAsync(id, produtoAtualizarDto);
            return Ok("Produto atualizado com sucesso...");
        }

        [HttpDelete]
        public async Task<ActionResult> RemoverProduto(Guid id)
        {
            await _produtoService.DeletarProdutoAsync(id);
            return Ok("Produto deletado com sucesso...");
        }
    }
}
