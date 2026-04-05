using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<ActionResult> CriarUsuario([FromBody] UsuarioCriarDto usuarioDto)
        {
            var usuario = await _usuarioService.CriarUsuarioAsync(usuarioDto);
            return CreatedAtAction(nameof(BuscarUsuarioPorId), new { id = usuario.UsuarioId }, usuario);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarUsuarios()
        {
            var usuarios = await _usuarioService.BuscarUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> BuscarUsuarioPorId(Guid id)
        {
            var usuario = await _usuarioService.BuscarUsuarioPorIdAsync(id);
            return Ok(usuario);
        }

        [HttpPut("{id:guid}/role")]
        public async Task<ActionResult> AtualizarRole(Guid id, [FromBody] UsuarioRoleAtualizarDto roleDto)
        {
            var usuario = await _usuarioService.AtualizarRoleAsync(id, roleDto);
            return Ok(usuario);
        }

        [HttpPut("{id:guid}/ativar")]
        public async Task<ActionResult> AtivarUsuario(Guid id)
        {
            var usuario = await _usuarioService.AlterarStatusAsync(id, true);
            return Ok(usuario);
        }

        [HttpPut("{id:guid}/desativar")]
        public async Task<ActionResult> DesativarUsuario(Guid id)
        {
            var usuario = await _usuarioService.AlterarStatusAsync(id, false);
            return Ok(usuario);
        }
    }
}
