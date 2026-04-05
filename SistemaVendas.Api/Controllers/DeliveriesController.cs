using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Route("api/deliveries")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<ActionResult> BuscarDeliveries()
        {
            var deliveries = await _deliveryService.BuscarDeliveriesAsync();
            return Ok(deliveries);
        }

        [HttpGet("pendentes")]
        public async Task<ActionResult> BuscarPendentes()
        {
            var deliveries = await _deliveryService.BuscarDeliveriesPendentesAsync();
            return Ok(deliveries);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            var delivery = await _deliveryService.BuscarDeliveryPorIdAsync(id);
            return Ok(delivery);
        }

        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult> AtualizarStatus(Guid id, [FromBody] DeliveryAtualizarStatusDto dto)
        {
            var delivery = await _deliveryService.AtualizarStatusAsync(id, dto);
            return Ok(delivery);
        }
    }
}
