using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    /// <summary>
    /// Endpoints para criacao, atribuicao e execucao de rotas.
    /// </summary>
    [ApiController]
    [Route("api/rotas")]
    public class RotasController : ControllerBase
    {
        private readonly IRotaService _rotaService;

        public RotasController(IRotaService rotaService)
        {
            _rotaService = rotaService;
        }

        /// <summary>
        /// Cria uma nova rota a partir de deliveries pendentes.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] RotaCriarDto dto)
        {
            var rota = await _rotaService.CriarRotaAsync(dto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = rota.RotaId }, rota);
        }

        /// <summary>
        /// Lista as rotas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> BuscarRotas()
        {
            var rotas = await _rotaService.BuscarRotasAsync();
            return Ok(rotas);
        }

        /// <summary>
        /// Busca uma rota pelo identificador.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            var rota = await _rotaService.BuscarRotaPorIdAsync(id);
            return Ok(rota);
        }

        /// <summary>
        /// Atribui a rota a um usuario entregador.
        /// </summary>
        [HttpPost("{id:guid}/atribuir/{entregadorId:guid}")]
        public async Task<ActionResult> Atribuir(Guid id, Guid entregadorId, [FromQuery] Guid alteradoPorUsuarioId)
        {
            var rota = await _rotaService.AtribuirEntregadorAsync(id, entregadorId, alteradoPorUsuarioId);
            return Ok(rota);
        }

        /// <summary>
        /// Reordena as paradas de uma rota editavel.
        /// </summary>
        [HttpPut("{id:guid}/paradas/reordenar")]
        public async Task<ActionResult> Reordenar(Guid id, [FromBody] RotaReordenarParadasDto dto)
        {
            var rota = await _rotaService.ReordenarParadasAsync(id, dto);
            return Ok(rota);
        }

        /// <summary>
        /// Inicia uma rota atribuida.
        /// </summary>
        [HttpPut("{id:guid}/iniciar")]
        public async Task<ActionResult> Iniciar(Guid id, [FromQuery] Guid alteradoPorUsuarioId)
        {
            var rota = await _rotaService.IniciarRotaAsync(id, alteradoPorUsuarioId);
            return Ok(rota);
        }

        /// <summary>
        /// Finaliza uma rota em progresso e bloqueia novas alteracoes.
        /// </summary>
        [HttpPut("{id:guid}/finalizar")]
        public async Task<ActionResult> Finalizar(Guid id, [FromQuery] Guid alteradoPorUsuarioId)
        {
            var rota = await _rotaService.FinalizarRotaAsync(id, alteradoPorUsuarioId);
            return Ok(rota);
        }
    }
}
