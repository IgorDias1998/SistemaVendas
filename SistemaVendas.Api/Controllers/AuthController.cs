using Microsoft.AspNetCore.Mvc;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Cria o primeiro usuario administrador quando ainda nao existem usuarios cadastrados.
        /// </summary>
        [HttpPost("bootstrap-admin")]
        public async Task<ActionResult> BootstrapAdmin([FromBody] UsuarioCriarDto usuarioDto)
        {
            var usuario = await _authService.BootstrapAdminAsync(usuarioDto);
            return CreatedAtAction(nameof(BootstrapAdmin), new { id = usuario.UsuarioId }, usuario);
        }

        /// <summary>
        /// Realiza o login do usuario e retorna o token JWT de acesso.
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AuthLoginDto loginDto)
        {
            var authResponse = await _authService.LoginAsync(loginDto);
            return Ok(authResponse);
        }
    }
}
