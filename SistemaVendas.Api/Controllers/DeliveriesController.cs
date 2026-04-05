using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Api.Extensions;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    /// <summary>
    /// Endpoints para consulta e atualizacao de deliveries.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Admin,Operador,Entregador")]
    [Route("api/deliveries")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        /// <summary>
        /// Lista todas as deliveries.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> BuscarDeliveries([FromQuery] DeliveryListQueryDto query)
        {
            var deliveries = await _deliveryService.BuscarDeliveriesAsync(User.GetRequiredUserId(), User.GetRequiredUserRole(), query);
            return Ok(deliveries);
        }

        /// <summary>
        /// Lista apenas deliveries pendentes.
        /// </summary>
        [HttpGet("pendentes")]
        public async Task<ActionResult> BuscarPendentes()
        {
            var deliveries = await _deliveryService.BuscarDeliveriesPendentesAsync(User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(deliveries);
        }

        /// <summary>
        /// Lista as deliveries relacionadas ao entregador autenticado.
        /// </summary>
        [Authorize(Roles = "Entregador")]
        [HttpGet("minhas")]
        public async Task<ActionResult> BuscarMinhasDeliveries()
        {
            var deliveries = await _deliveryService.BuscarDeliveriesAsync(User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(deliveries);
        }

        /// <summary>
        /// Busca uma delivery pelo identificador.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            var delivery = await _deliveryService.BuscarDeliveryPorIdAsync(id, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(delivery);
        }

        /// <summary>
        /// Atualiza o status de uma delivery.
        /// </summary>
        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult> AtualizarStatus(Guid id, [FromBody] DeliveryAtualizarStatusDto dto)
        {
            var delivery = await _deliveryService.AtualizarStatusAsync(id, dto, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(delivery);
        }

        /// <summary>
        /// Registra falha de uma entrega com motivo.
        /// </summary>
        [HttpPut("{id:guid}/falha")]
        public async Task<ActionResult> RegistrarFalha(Guid id, [FromBody] RegistrarFalhaEntregaDto dto)
        {
            var delivery = await _deliveryService.RegistrarFalhaAsync(id, dto, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(delivery);
        }
    }
}
