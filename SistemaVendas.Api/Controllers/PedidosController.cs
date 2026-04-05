using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Api.Extensions;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    /// <summary>
    /// Endpoints para criacao, consulta e mudanca de status dos pedidos.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Admin,Operador")]
    [Route("api/pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Cria um pedido em rascunho.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CriarRascunho([FromBody] PedidoCriarDto pedidoDto)
        {
            var pedido = await _pedidoService.CriarRascunhoAsync(pedidoDto, User.GetRequiredUserId());
            return CreatedAtAction(nameof(BuscarPorId), new { id = pedido.PedidoId }, pedido);
        }

        /// <summary>
        /// Lista os pedidos cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> BuscarPedidos()
        {
            var pedidos = await _pedidoService.BuscarPedidosAsync();
            return Ok(pedidos);
        }

        /// <summary>
        /// Busca um pedido pelo identificador.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            var pedido = await _pedidoService.BuscarPedidoPorIdAsync(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Confirma um pedido e gera delivery quando o tipo for delivery.
        /// </summary>
        [HttpPut("{id:guid}/confirmar")]
        public async Task<ActionResult> Confirmar(Guid id)
        {
            var pedido = await _pedidoService.ConfirmarPedidoAsync(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Cancela um pedido e a delivery associada, quando existir.
        /// </summary>
        [HttpPut("{id:guid}/cancelar")]
        public async Task<ActionResult> Cancelar(Guid id)
        {
            var pedido = await _pedidoService.CancelarPedidoAsync(id);
            return Ok(pedido);
        }
    }
}
