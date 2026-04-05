using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Api.Extensions;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    /// <summary>
    /// Endpoints para criacao, atribuicao e execucao de rotas.
    /// </summary>
    [ApiController]
    [Route("api/rotas")]
    [Authorize(Roles = "Admin,Operador,Entregador")]
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
        [Authorize(Roles = "Admin,Operador")]
        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] RotaCriarDto dto)
        {
            var rota = await _rotaService.CriarRotaAsync(dto, User.GetRequiredUserId());
            return CreatedAtAction(nameof(BuscarPorId), new { id = rota.RotaId }, rota);
        }

        /// <summary>
        /// Lista as rotas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> BuscarRotas([FromQuery] RotaListQueryDto query)
        {
            var rotas = await _rotaService.BuscarRotasAsync(User.GetRequiredUserId(), User.GetRequiredUserRole(), query);
            return Ok(rotas);
        }

        /// <summary>
        /// Lista as rotas do entregador autenticado.
        /// </summary>
        [Authorize(Roles = "Entregador")]
        [HttpGet("minhas")]
        public async Task<ActionResult> BuscarMinhasRotas()
        {
            var rotas = await _rotaService.BuscarRotasAsync(User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(rotas);
        }

        /// <summary>
        /// Busca uma rota pelo identificador.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            var rota = await _rotaService.BuscarRotaPorIdAsync(id, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(rota);
        }

        /// <summary>
        /// Lista os logs de auditoria de uma rota.
        /// </summary>
        [HttpGet("{id:guid}/logs")]
        public async Task<ActionResult> BuscarLogs(Guid id)
        {
            var logs = await _rotaService.BuscarLogsAsync(id, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(logs);
        }

        /// <summary>
        /// Atribui a rota a um usuario entregador.
        /// </summary>
        [Authorize(Roles = "Admin,Operador")]
        [HttpPost("{id:guid}/atribuir/{entregadorId:guid}")]
        public async Task<ActionResult> Atribuir(Guid id, Guid entregadorId)
        {
            var rota = await _rotaService.AtribuirEntregadorAsync(id, entregadorId, User.GetRequiredUserId());
            return Ok(rota);
        }

        /// <summary>
        /// Reordena as paradas de uma rota editavel.
        /// </summary>
        [Authorize(Roles = "Admin,Operador")]
        [HttpPut("{id:guid}/paradas/reordenar")]
        public async Task<ActionResult> Reordenar(Guid id, [FromBody] RotaReordenarParadasDto dto)
        {
            var rota = await _rotaService.ReordenarParadasAsync(id, dto, User.GetRequiredUserId());
            return Ok(rota);
        }

        /// <summary>
        /// Marca uma parada como concluida e a delivery como entregue.
        /// </summary>
        [Authorize(Roles = "Admin,Operador,Entregador")]
        [HttpPut("{id:guid}/paradas/{paradaId:guid}/concluir")]
        public async Task<ActionResult> ConcluirParada(Guid id, Guid paradaId)
        {
            var rota = await _rotaService.ConcluirParadaAsync(id, paradaId, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(rota);
        }

        /// <summary>
        /// Registra falha da entrega vinculada a parada.
        /// </summary>
        [Authorize(Roles = "Admin,Operador,Entregador")]
        [HttpPut("{id:guid}/paradas/{paradaId:guid}/falha")]
        public async Task<ActionResult> RegistrarFalhaParada(Guid id, Guid paradaId, [FromBody] RegistrarFalhaEntregaDto dto)
        {
            var rota = await _rotaService.RegistrarFalhaParadaAsync(id, paradaId, dto, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(rota);
        }

        /// <summary>
        /// Inicia uma rota atribuida.
        /// </summary>
        [Authorize(Roles = "Admin,Operador,Entregador")]
        [HttpPut("{id:guid}/iniciar")]
        public async Task<ActionResult> Iniciar(Guid id)
        {
            var rota = await _rotaService.IniciarRotaAsync(id, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(rota);
        }

        /// <summary>
        /// Finaliza uma rota em progresso e bloqueia novas alteracoes.
        /// </summary>
        [Authorize(Roles = "Admin,Operador,Entregador")]
        [HttpPut("{id:guid}/finalizar")]
        public async Task<ActionResult> Finalizar(Guid id)
        {
            var rota = await _rotaService.FinalizarRotaAsync(id, User.GetRequiredUserId(), User.GetRequiredUserRole());
            return Ok(rota);
        }
    }
}
