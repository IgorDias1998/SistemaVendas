using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> AdicionarAsync(Usuario usuario);
        Task<Usuario?> BuscarPorIdAsync(Guid usuarioId);
        Task<Usuario?> BuscarPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> BuscarTodosAsync();
        Task<bool> ExisteAlgumUsuarioAsync();
        Task AtualizarAsync(Usuario usuario);
    }
}
