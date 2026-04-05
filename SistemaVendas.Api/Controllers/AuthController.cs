using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Api.Extensions;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Api.Controllers
{
    /// <summary>
    /// Endpoints de autenticacao e bootstrap inicial do sistema.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;

        public AuthController(IAuthService authService, IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Cria o primeiro usuario administrador quando ainda nao existem usuarios cadastrados.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("bootstrap-admin")]
        public async Task<ActionResult> BootstrapAdmin([FromBody] UsuarioCriarDto usuarioDto)
        {
            var usuario = await _authService.BootstrapAdminAsync(usuarioDto);
            return CreatedAtAction(nameof(BootstrapAdmin), new { id = usuario.UsuarioId }, usuario);
        }

        /// <summary>
        /// Realiza o login do usuario e retorna o token JWT de acesso.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AuthLoginDto loginDto)
        {
            var authResponse = await _authService.LoginAsync(loginDto);
            return Ok(authResponse);
        }

        /// <summary>
        /// Retorna os dados do usuario autenticado.
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UsuarioReadDto>> Me()
        {
            var usuario = await _usuarioService.BuscarUsuarioPorIdAsync(User.GetRequiredUserId());
            return Ok(usuario);
        }
    }
}
