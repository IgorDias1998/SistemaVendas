using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UsuarioReadDto> BootstrapAdminAsync(UsuarioCriarDto usuarioDto);
        Task<AuthResponseDto> LoginAsync(AuthLoginDto loginDto);
    }
}
