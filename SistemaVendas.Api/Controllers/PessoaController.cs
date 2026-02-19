using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Route("api/pessoa")]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpPost]
        public async Task<ActionResult> CriarNovoCadastroPessoaAsync([FromBody] PessoaCreateDto pessoaCreateDto)
        {
            await _pessoaService.CriarPessoaAsync(pessoaCreateDto);
            return Ok("Pessoa cadastrada com sucesso.");
        }

        [HttpGet]
        public async Task<ActionResult> BuscarTodasPessoasAsync()
        {
            var pessoas = await _pessoaService.BuscarPessoasAsync();
            return Ok(pessoas);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPessoaPorIdAsync(Guid id)
        {
            var pessoa = await _pessoaService.BuscarPessoaIdAsync(id);
            return Ok(pessoa);
        }

        [HttpPut]
        public async Task<ActionResult> AtualizarPessoaAsync(Guid id, PessoaAtualizarDto pessoaAtualizarDto)
        {
            await _pessoaService.AtualizarPessoaAsync(id, pessoaAtualizarDto);
            return Ok("Pessoa atualizada com sucesso.");
        }

        [HttpDelete]
        public async Task<ActionResult> RemoverPessoaAsync(Guid id)
        {
            await _pessoaService.DeletarPessoaAsync(id);
            return Ok("Cadastro de pessoa removido com sucesso.");
        }
    }
}
