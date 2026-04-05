using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioReadDto> CriarUsuarioAsync(UsuarioCriarDto usuarioDto);
        Task<IEnumerable<UsuarioReadDto>> BuscarUsuariosAsync();
        Task<UsuarioReadDto> BuscarUsuarioPorIdAsync(Guid usuarioId);
        Task<UsuarioReadDto> AtualizarRoleAsync(Guid usuarioId, UsuarioRoleAtualizarDto roleDto);
        Task<UsuarioReadDto> AlterarStatusAsync(Guid usuarioId, bool ativo);
    }
}
