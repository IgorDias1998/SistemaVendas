using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<ActionResult> CriarRascunho([FromBody] PedidoCriarDto pedidoDto)
        {
            var pedido = await _pedidoService.CriarRascunhoAsync(pedidoDto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = pedido.PedidoId }, pedido);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarPedidos()
        {
            var pedidos = await _pedidoService.BuscarPedidosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            var pedido = await _pedidoService.BuscarPedidoPorIdAsync(id);
            return Ok(pedido);
        }

        [HttpPut("{id:guid}/confirmar")]
        public async Task<ActionResult> Confirmar(Guid id)
        {
            var pedido = await _pedidoService.ConfirmarPedidoAsync(id);
            return Ok(pedido);
        }

        [HttpPut("{id:guid}/cancelar")]
        public async Task<ActionResult> Cancelar(Guid id)
        {
            var pedido = await _pedidoService.CancelarPedidoAsync(id);
            return Ok(pedido);
        }
    }
}
