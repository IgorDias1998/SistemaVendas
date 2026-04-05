using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Route("api/rotas")]
    public class RotasController : ControllerBase
    {
        private readonly IRotaService _rotaService;

        public RotasController(IRotaService rotaService)
        {
            _rotaService = rotaService;
        }

        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] RotaCriarDto dto)
        {
            var rota = await _rotaService.CriarRotaAsync(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = rota.RotaId }, rota);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarRotas()
        {
            var rotas = await _rotaService.BuscarRotasAsync();
            return Ok(rotas);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            var rota = await _rotaService.BuscarRotaPorIdAsync(id);
            return Ok(rota);
        }

        [HttpPost("{id:guid}/atribuir/{entregadorId:guid}")]
        public async Task<ActionResult> Atribuir(Guid id, Guid entregadorId)
        {
            var rota = await _rotaService.AtribuirEntregadorAsync(id, entregadorId);
            return Ok(rota);
        }

        [HttpPut("{id:guid}/paradas/reordenar")]
        public async Task<ActionResult> Reordenar(Guid id, [FromBody] RotaReordenarParadasDto dto)
        {
            var rota = await _rotaService.ReordenarParadasAsync(id, dto);
            return Ok(rota);
        }

        [HttpPut("{id:guid}/iniciar")]
        public async Task<ActionResult> Iniciar(Guid id)
        {
            var rota = await _rotaService.IniciarRotaAsync(id);
            return Ok(rota);
        }

        [HttpPut("{id:guid}/finalizar")]
        public async Task<ActionResult> Finalizar(Guid id)
        {
            var rota = await _rotaService.FinalizarRotaAsync(id);
            return Ok(rota);
        }
    }
}
