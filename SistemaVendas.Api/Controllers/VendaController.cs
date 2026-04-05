using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Operador")]
    [Route("api/vendas")]
    public class VendaController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendaController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarVenda([FromBody] VendaCriarDto vendaDto)
        {
            var vendaCriada = await _vendaService.AdicionarVendaAsync(vendaDto);
            return CreatedAtAction(nameof(BuscarVendaPorId), new { id = vendaCriada.VendaId }, vendaCriada);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarVendas()
        {
            var vendas = await _vendaService.BuscarVendasAsync();
            return Ok(vendas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> BuscarVendaPorId(int id)
        {
            var venda = await _vendaService.BuscarVendaPorIdAsync(id);
            return Ok(venda);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> AtualizarVenda(int id, [FromBody] VendaAtualizarDto vendaDto)
        {
            var vendaAtualizada = await _vendaService.AtualizarVendaAsync(id, vendaDto);
            return Ok(vendaAtualizada);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoverVenda(int id)
        {
            await _vendaService.DeletarVendaAsync(id);
            return NoContent();
        }
    }
}
