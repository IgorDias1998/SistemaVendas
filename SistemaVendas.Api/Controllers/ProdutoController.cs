using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IProdutoImportService _importService;

        public ProdutoController(IProdutoService produtoService, IProdutoImportService importService)
        {
            _produtoService = produtoService;
            _importService = importService;
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarProduto([FromBody] ProdutoCriarDto produtoDto)
        {
            var produtoCriado = await _produtoService.CriarProdutoAsync(produtoDto);
            return CreatedAtAction(nameof(BuscarProdutoPorId), new { id = produtoCriado.ProdutoId }, produtoCriado);
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
            var produtoAtualizado = await _produtoService.AtualizarProdutoAsync(id, produtoAtualizarDto);
            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> RemoverProduto(Guid id)
        {
            await _produtoService.DeletarProdutoAsync(id);
            return NoContent();
        }

        [HttpPost("importarCSV")]
        public async Task<IActionResult> ImportarProdutos(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido.");

            await _importService.ImportarAsync(file.OpenReadStream());

            return Ok("Produtos importados com sucesso.");
        }
    }
}
