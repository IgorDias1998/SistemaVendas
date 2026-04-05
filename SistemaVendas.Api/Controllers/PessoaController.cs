using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<ActionResult> CriarClienteAsync([FromBody] ClienteCreateDto clienteCreateDto)
        {
            var cliente = await _clienteService.CriarClienteAsync(clienteCreateDto);
            return CreatedAtAction(nameof(BuscarClientePorIdAsync), new { id = cliente.ClienteId }, cliente);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarClientesAsync()
        {
            var clientes = await _clienteService.BuscarClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarClientePorIdAsync(Guid id)
        {
            var cliente = await _clienteService.BuscarClientePorIdAsync(id);
            return Ok(cliente);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> AtualizarClienteAsync(Guid id, [FromBody] ClienteAtualizarDto clienteAtualizarDto)
        {
            var cliente = await _clienteService.AtualizarClienteAsync(id, clienteAtualizarDto);
            return Ok(cliente);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> RemoverClienteAsync(Guid id)
        {
            await _clienteService.DeletarClienteAsync(id);
            return NoContent();
        }
    }
}
